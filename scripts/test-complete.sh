#!/usr/bin/env bash

set -e

echo "------------------------------------------------"
echo "-  Running Test on all Service Functionality   -"
echo "------------------------------------------------"
echo 

SCRIPT_DIR="$( cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$( cd "$SCRIPT_DIR/.." && pwd)"

cd "$REPO_ROOT"

echo "Running Dependency Check..."
echo

echo "[1/3] Restoring Dependencies..."
dotnet restore PortfolioWork.sln

echo
echo "[2/3] Building Solution..."
dotnet build PortfolioWork.sln --no-restore

echo
echo "[3/3] Running Dependency Tests"
dotnet test PortfolioWork.sln --no-build --no-restore

echo
echo "------------------------------------------------"
echo "-            All Tests COMPLETED               -"
echo "------------------------------------------------"