#!/usr/bin/env bash

set -e

# --------- The following would test all files in the PortfolioWork solution ---------

#    echo "----------------------------------------------"
#    echo "-     Running Detailed Diagnostic Test       -"
#    echo "----------------------------------------------"
#    echo 

#    SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
#    REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

#    cd "$REPO_ROOT"

#    dotnet test PortfolioWork.sln --logger 'console;verbosity=detailed'
	
#    echo 
#    echo "----------------------------------------------"
#    echo "-       All Diagnostics Have Passed          -"
#    echo "----------------------------------------------"


# ------------ Shared Directory Tests ------------

echo
echo "================================================"
echo " Performing Testing: Shared Directory  "
echo "================================================"

echo
echo "================================================"
echo " Testing: ErrorCodeStatusCodeMap"
echo " Purpose: HTTP error/status translation"
echo "================================================"

dotnet test tests/Shared.Tests/Shared.Tests.csproj \
    --filter "FullyQualifiedName~Shared.Tests.ErrorCodeStatusCodeMapTests" \
    --logger 'console;verbosity=detailed'

echo
echo "================================================"
echo " Testing: Result"
echo " Purpose: Service operation result contracts"
echo "================================================"

dotnet test tests/Shared.Tests/Shared.Tests.csproj \
    --filter "FullyQualifiedName~Shared.Tests.ResultTests" \
    --logger 'console;verbosity=detailed'

echo
echo "================================================"
echo " Testing: ContractSerialization"
echo " Purpose: Ensuring the APIResponse and APIError viability"
echo "================================================"

dotnet test tests/Shared.Tests/Shared.Tests.csproj \
    --filter "FullyQualifiedName~Shared.Tests.ContractSerialization" \
    --logger 'console;verbosity=detailed'

echo
echo "==================================================="
echo " Shared Directory Testing Completed Successfully!  "
echo "==================================================="