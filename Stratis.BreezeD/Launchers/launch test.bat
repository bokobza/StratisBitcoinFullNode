cd ..
start cmd /k dotnet run light -testnet -debug=all -loglevel=debug
timeout 7
start cmd /k dotnet run stratis light -testnet -debug=all -loglevel=debug