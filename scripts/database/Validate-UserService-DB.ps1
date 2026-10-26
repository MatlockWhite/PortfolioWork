
# ============================================================
# PortfolioWork - UserService Database Validation
# ============================================================
# Purpose:
#   Verify that the Dockerized UserService database has been initialized and configured correctly.
#
# Demonstration Note:
#   This script is intended for the PortfolioWork educational environment.
# ============================================================

$ErrorActionPreference = "Stop"

# Resolve the repository root relative to this script.
$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path

# Run Compose commands against the correct project.
Push-Location $repositoryRoot

try {
	
Write-Host "`n=== UserService Database Validation ===" -ForegroundColor Cyan

# Note: This script assumes that the user has ran the command "docker compose start" 

# Test 1: Verify that the MySQL container is properly established.

$containerId = docker compose ps -q mysql

if ($LASTEXITCODE -ne 0 -or -not $containerId) {
    Write-Host "[FAIL] MySQL container was not found. Please Verify that the Docker container is currently running. (Command: docker compose start)" -ForegroundColor Red
    throw "Database validation failed."
}

$health = docker inspect --format '{{.State.Health.Status}}' $containerId

if ($LASTEXITCODE -ne 0 -or $health -ne "healthy") {
    Write-Host "[FAIL] MySQL container is not properly established." -ForegroundColor Red
    throw "Database validation failed."
}

Write-Host "[PASS] MySQL container successfully established." -ForegroundColor Green


# Test 2: Verify that the required database tables exist.

$requiredTables = @("role", "user")

# The following use of MYSQL_PWD avoids passing the password as a command-line argument.. 
# This is for demonstration purposes only as we are not requiring the end user to make their own password to test the functionality of this Portfolio Project.
$tableResults = docker compose exec -T `
    -e MYSQL_PWD=password `
    mysql mysql `
    -uReadWriteUser -D UserService `
    --batch --skip-column-names `
    -e "SHOW TABLES;"

if ($LASTEXITCODE -ne 0) {
    Write-Host "[FAIL] Unable to query database tables." -ForegroundColor Red
    throw "Database validation failed."
}

foreach ($table in $requiredTables) {
    if ($tableResults -contains $table) {
        Write-Host "[PASS] Table '$table' exists." -ForegroundColor Green
    }
    else {
        Write-Host "[FAIL] Table '$table' is missing." -ForegroundColor Red
        throw "Database validation failed."
    }
}

# Test 3: Verify that the database contains the expected seed data.

# We have 4 Roles (Administrator, Supervisor, Warehouse Worker, and Driver) & 9 Users with various roles for our automated Testing purposes.
$expectedCounts = @{
    role = 4
    user = 9
}

foreach ($table in @("role", "user")) {

    $result = docker compose exec -T `
        -e MYSQL_PWD=password `
        mysql mysql `
        -uReadWriteUser -D UserService `
        --batch --skip-column-names `
        -e "SELECT COUNT(*) FROM ``$table``;"

    if ($LASTEXITCODE -ne 0) {
        Write-Host "[FAIL] Unable to query '$table' records." -ForegroundColor Red
        throw "Database validation failed."
    }

    $actualCount = 0

    if (-not [int]::TryParse(($result | Out-String).Trim(), [ref]$actualCount)) {
        Write-Host "[FAIL] Invalid record count returned for '$table'." -ForegroundColor Red
        throw "Database validation failed."
    }

    if ($actualCount -eq $expectedCounts[$table]) {
        Write-Host "[PASS] Table '$table' contains the expected: $actualCount records." -ForegroundColor Green
    }
    else {
        Write-Host "[FAIL] Table '$table' contains: $actualCount records, expected: $($expectedCounts[$table])." -ForegroundColor Red
        throw "Database validation failed."
    }
}

# Test 4: Verify that all assigned user roles reference valid roles.

$query = @"
SELECT COUNT(*)
FROM ``user`` u
LEFT JOIN ``role`` r ON u.RoleId = r.Id
WHERE u.RoleId IS NOT NULL
  AND r.Id IS NULL;
"@

$result = docker compose exec -T `
    -e MYSQL_PWD=password `
    mysql mysql `
    -uReadWriteUser -D UserService `
    --batch --skip-column-names `
    -e $query

if ($LASTEXITCODE -ne 0) {
    Write-Host "[FAIL] Unable to validate user role references." -ForegroundColor Red
    throw "Database validation failed."
}

$invalidReferences = 0

if (-not [int]::TryParse(($result | Out-String).Trim(), [ref]$invalidReferences)) {
    Write-Host "[FAIL] Invalid foreign key validation result." -ForegroundColor Red
    throw "Database validation failed."
}

if ($invalidReferences -eq 0) {
    Write-Host "[PASS] All assigned user roles reference valid roles." -ForegroundColor Green
}
else {
    Write-Host "[FAIL] Found $invalidReferences invalid user role references." -ForegroundColor Red
    throw "Database validation failed."
}

# Test 5: Verify database user permissions.
# Each account should have only its intended table privileges.
# We currently have 4 Users but are only testing the functionality of the core 3: (ReadUser, WriteUser, ReadWriteUser), Administrator is responsible for DB Schema manipulation and not Data.

$expectedGrants = @{
    ReadUser      = @("SELECT")
    WriteUser     = @("INSERT", "UPDATE", "DELETE")
    ReadWriteUser = @("SELECT", "INSERT", "UPDATE", "DELETE")
}

foreach ($dbUser in @("ReadUser", "WriteUser", "ReadWriteUser")) {
    $query = @"
SELECT PRIVILEGE_TYPE
FROM information_schema.SCHEMA_PRIVILEGES
WHERE GRANTEE = CONCAT(CHAR(39), '$dbUser', CHAR(39), '@', CHAR(39), '%', CHAR(39))
  AND TABLE_SCHEMA = 'UserService'
ORDER BY PRIVILEGE_TYPE;
"@
	# Root is used here for testing purposes only to audit database account permissions.
	# Application queries continue to use appropriate user credentials.
	# Production environments should use a dedicated audit account with only the permissions required for validation.
	$result = docker compose exec -T `
		-e MYSQL_PWD=password `
		mysql mysql `
		-uroot -D UserService `
		--batch --skip-column-names `
		-e $query

    if ($LASTEXITCODE -ne 0) {
        Write-Host "[FAIL] Unable to validate permissions for '$dbUser'." -ForegroundColor Red
        throw "Database validation failed."
    }

    $actual = @($result | Where-Object { $_ -and $_.Trim() })
    $expected = @($expectedGrants[$dbUser])

    $missing = @($expected | Where-Object { $_ -notin $actual })
    $unexpected = @($actual | Where-Object { $_ -notin $expected })

    if ($missing.Count -eq 0 -and $unexpected.Count -eq 0) {
        Write-Host "[PASS] '$dbUser' has the expected database privileges." -ForegroundColor Green
    }
    else {
        Write-Host "[FAIL] '$dbUser' has incorrect database privileges." -ForegroundColor Red
        Write-Host "       Missing: $($missing -join ', ')"
        Write-Host "       Unexpected: $($unexpected -join ', ')"
        throw "Database validation failed."
    }
}

# Test 6: Verify application accounts have no global privileges.
# USAGE is allowed because it does not grant data access.

foreach ($dbUser in @("ReadUser", "WriteUser", "ReadWriteUser")) {

    $query = @"
SELECT PRIVILEGE_TYPE
FROM information_schema.USER_PRIVILEGES
WHERE GRANTEE = CONCAT(CHAR(39), '$dbUser', CHAR(39), '@', CHAR(39), '%', CHAR(39))
  AND PRIVILEGE_TYPE <> 'USAGE';
"@

    $result = docker compose exec -T `
        -e MYSQL_PWD=password `
        mysql mysql `
        -uroot -D UserService `
        --batch --skip-column-names `
        -e $query

    if ($LASTEXITCODE -ne 0) {
        Write-Host "[FAIL] Unable to audit global privileges for '$dbUser'." -ForegroundColor Red
		$UserServiceValidationStatus = "FAILED"
        throw "Database validation failed."
    }

    $unexpected = @($result | Where-Object { $_ -and $_.Trim() })

    if ($unexpected.Count -eq 0) {
        Write-Host "[PASS] '$dbUser' has no unexpected global privileges." -ForegroundColor Green
    }
    else {
        Write-Host "[FAIL] '$dbUser' has unexpected global privileges: $($unexpected -join ', ')" -ForegroundColor Red
        throw "Database validation failed."
    }
}

# Test 7: Verify application accounts have no table-level grants.
# Application permissions are intentionally assigned at the UserService database level, not individual table level.

foreach ($dbUser in @("ReadUser", "WriteUser", "ReadWriteUser")) {

    $query = @"
SELECT CONCAT(TABLE_SCHEMA, '.', TABLE_NAME, ': ', PRIVILEGE_TYPE)
FROM information_schema.TABLE_PRIVILEGES
WHERE GRANTEE = CONCAT(CHAR(39), '$dbUser', CHAR(39), '@', CHAR(39), '%', CHAR(39));
"@

    $result = docker compose exec -T `
        -e MYSQL_PWD=password `
        mysql mysql `
        -uroot -D UserService `
        --batch --skip-column-names `
        -e $query

    if ($LASTEXITCODE -ne 0) {
        Write-Host "[FAIL] Unable to audit table-level privileges for '$dbUser'." -ForegroundColor Red
        throw "Database validation failed."
    }

    $unexpected = @($result | Where-Object { $_ -and $_.Trim() })

    if ($unexpected.Count -eq 0) {
        Write-Host "[PASS] '$dbUser' has no unexpected table-level privileges." -ForegroundColor Green
    }
    else {
        Write-Host "[FAIL] '$dbUser' has unexpected table-level privileges:" -ForegroundColor Red

        foreach ($privilege in $unexpected) {
            Write-Host "       $privilege"
        }

        throw "Database validation failed."
    }
}

# Test 8: Verify no column-level privileges exist.

foreach ($dbUser in @("ReadUser", "WriteUser", "ReadWriteUser")) {

    $query = @"
SELECT CONCAT(TABLE_SCHEMA, '.', TABLE_NAME, '.', COLUMN_NAME, ': ', PRIVILEGE_TYPE)
FROM information_schema.COLUMN_PRIVILEGES
WHERE GRANTEE = CONCAT(CHAR(39), '$dbUser', CHAR(39), '@', CHAR(39), '%', CHAR(39));
"@

    $result = docker compose exec -T `
        -e MYSQL_PWD=password `
        mysql mysql `
        -uroot -D UserService `
        --batch --skip-column-names `
        -e $query

    if ($LASTEXITCODE -ne 0) {
        Write-Host "[FAIL] Unable to audit column privileges for '$dbUser'." -ForegroundColor Red
        throw "Database validation failed."
    }

    $unexpected = @($result | Where-Object { $_ -and $_.Trim() })

    if ($unexpected.Count -gt 0) {
        Write-Host "[FAIL] '$dbUser' has unexpected column privileges: $($unexpected -join ', ')" -ForegroundColor Red
        throw "Database validation failed."
    }

    Write-Host "[PASS] '$dbUser' has no unexpected column privileges." -ForegroundColor Green
}

# Test 9: 
# Verify application accounts have no assigned MySQL roles.
# Application accounts should receive only their explicitly configured privileges, without inherited role permissions.

foreach ($dbUser in @("ReadUser", "WriteUser", "ReadWriteUser")) {

    $query = @"
SELECT COUNT(*)
FROM mysql.role_edges
WHERE TO_USER = '$dbUser'
  AND TO_HOST = '%';
"@

    $result = docker compose exec -T `
        -e MYSQL_PWD=password `
        mysql mysql `
        -uroot -D UserService `
        --batch --skip-column-names `
        -e $query

    if ($LASTEXITCODE -ne 0) {
        Write-Host "[FAIL] Unable to audit MySQL roles for '$dbUser'." -ForegroundColor Red
        throw "Database validation failed."
    }

    $assignedRoles = 0

    if (-not [int]::TryParse(($result | Out-String).Trim(), [ref]$assignedRoles)) {
        Write-Host "[FAIL] Invalid role audit result for '$dbUser'." -ForegroundColor Red
        throw "Database validation failed."
    }

    if ($assignedRoles -eq 0) {
        Write-Host "[PASS] '$dbUser' has no assigned MySQL roles." -ForegroundColor Green
    }
    else {
        Write-Host "[FAIL] '$dbUser' has $assignedRoles unexpected MySQL role assignments." -ForegroundColor Red
        throw "Database validation failed."
    }
}
}
catch {
    Write-Host "`n========================================" -ForegroundColor Red
    Write-Host " UserService Database Validation: FAILED" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red

    # validation failure messages.
    if ($_.Exception.Message -ne "Database validation failed.") {
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    }

    $validationFailed = $true
}
finally {
    Pop-Location
}

if ($validationFailed){
	exit 1
}

Write-Host " `n === All Tests Completed ===" -ForegroundColor Cyan
Write-Host "`n============================================================" -ForegroundColor Cyan
Write-Host " UserService Database Validation: PASSED" -ForegroundColor Green
Write-Host "==============================================================" -ForegroundColor Cyan
	
exit 0