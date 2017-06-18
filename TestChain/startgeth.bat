RD /S /Q %~dp0\devChain\geth

geth.exe --datadir=devChain init genesis_dev.json
geth.exe --mine --networkid=12345 --cache=2048 --maxpeers=0 --datadir=devChain --rpc --rpccorsdomain "*" --rpcapi "eth,web3,personal,net,miner,admin,debug" --verbosity 0 console  

REM geth.exe --rinkeby --syncmode "fast" --datadir=rinky --mine console
REM geth.exe --testnet --syncmode "fast" --datadir=testnet --verbosity 0 console
