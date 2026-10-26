/******************************************************************************
 * PortfolioWork - UserService Database Accounts
 *
 * PURPOSE:
 * Demonstrates MySQL account creation and least-privilege access control.
 *
 * SECURITY NOTICE:
 * Passwords are intentionally simplified and hardcoded for demonstration.
 * These are fictional credentials used ONLY for this local Docker environment.
 *
 * PRODUCTION CONSIDERATIONS:
 * - We would never actually commit real credentials to source control.
 * - Use a secrets manager or securely supplied environment variables.
 * - Restrict database accounts to trusted hosts.
 * - Grant only the permissions necessary for each account.
 * - Never reuse these demonstration credentials in production.
 ******************************************************************************/

-- Read-only account
CREATE USER IF NOT EXISTS 'ReadUser'@'%'
    IDENTIFIED BY 'password';

GRANT SELECT
ON `UserService`.*
TO 'ReadUser'@'%';


-- Write-only account
CREATE USER IF NOT EXISTS 'WriteUser'@'%'
    IDENTIFIED BY 'password';

GRANT INSERT, UPDATE, DELETE
ON `UserService`.*
TO 'WriteUser'@'%';


-- Read/write account
CREATE USER IF NOT EXISTS 'ReadWriteUser'@'%'
    IDENTIFIED BY 'password';

GRANT SELECT, INSERT, UPDATE, DELETE
ON `UserService`.*
TO 'ReadWriteUser'@'%';


-- Schema administrator account
CREATE USER IF NOT EXISTS 'Administrator'@'%'
    IDENTIFIED BY 'password';

GRANT CREATE, ALTER, DROP, INDEX, REFERENCES
ON `UserService`.*
TO 'Administrator'@'%';