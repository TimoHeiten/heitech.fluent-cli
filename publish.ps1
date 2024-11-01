
param(
    [string] $c = "Debug"
)

function checkExitCode([string] $message) {
    $exitCode = $LastExitCode
    if ($exitCode -ne 0) {
        Write-Host "Error: $message"
        exit $exitCode
    }
}

# clean up
dotnet clean
checkExitCode "Clean failed"

# build sln
dotnet build -c $c
checkExitCode "Build failed"

# test sln
dotnet test
checkExitCode "Test failed"