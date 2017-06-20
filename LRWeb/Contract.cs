using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LRWeb
{
    public class Contract
    {
        public string Abi { get; set; }
        public string ByteCode { get; set; }
        public string SenderAddress { get; set; }
        public string Password { get; set; }
        public string ContractAddress { get; set; }

        private Web3Geth Web3;

        public Contract(string senderAddress, string password)
        {
            this.SenderAddress = senderAddress;
            this.Password = password;

            this.Web3 = new Web3Geth();
        }

        public void Compile(string code)
        {
            Process proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = @"D:\Prasanna\BlockChain\Repos\Compiler\solc.exe",
                    Arguments = @"--bin --abi -",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            StreamWriter sw = proc.StandardInput;
            sw.WriteLine(code);
            sw.Dispose();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                // do something with line
                if (line == "Binary: ")
                {
                    this.ByteCode = "0x" + proc.StandardOutput.ReadLine();
                }
                else if (line == "Contract JSON ABI")
                {
                    this.Abi = proc.StandardOutput.ReadLine();
                }
            }
            proc.Dispose();
        }

        public async Task Deploy()
        {
            await UnlockAccount();

            string transactionHash =
                await this.Web3.Eth.DeployContract.SendRequestAsync(this.Abi, this.ByteCode, this.SenderAddress, new HexBigInteger(300000));
            var receipt = await this.Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            while (receipt == null)
            {
                Thread.Sleep(5000);
                receipt = await this.Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            this.ContractAddress = receipt.ContractAddress;
        }

        public async Task<string> SendTransaction(string abi, string contractAddress, string functionName, params object[] functionInput)
        {
            await UnlockAccount();

            var contract = this.Web3.Eth.GetContract(abi, contractAddress);
            var function = contract.GetFunction(functionName);
            return await function.SendTransactionAsync(this.SenderAddress, new HexBigInteger(30000), new HexBigInteger(30000), functionInput);
        }

        public async Task<T> Call<T>(string functionName, params object[] functionInput)
        {
            var contract = this.Web3.Eth.GetContract(this.Abi, this.ContractAddress);

            var function = contract.GetFunction(functionName);

            return await function.CallAsync<T>(functionInput);
        }

        private async Task UnlockAccount()
        {
            await this.Web3.Personal.UnlockAccount.SendRequestAsync(this.SenderAddress, this.Password, 120);
        }
    }
}
