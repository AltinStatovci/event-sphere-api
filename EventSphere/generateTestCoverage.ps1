# Ensure the script stops on errors
$ErrorActionPreference = "Stop"

$testProjectPath = "EventSphere.Tests"
cd $testProjectPath

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

# Navigate back to the root directory
cd ..

# Generate coverage report
reportgenerator -reports:$testProjectPath/coverage.cobertura.xml -targetdir:$testProjectPath/coverage-report

# Open the coverage report in the default browser
Start-Process "$testProjectPath/coverage-report/index.html"
