using System.Text;
using System.Threading.Tasks;

namespace LRWeb
{
    public class ProfileContract
    {
        public static string ContractAddress;
        public static string Abi;

        // local Address
        private string senderAddress = "0x0bd615694d433424c292599f080bda45b0c257fc";
        // Rinkeby Address
        // private string senderAddress = "0x65873e9f02633f8b87ca1896bd811c58ad000a15";
        private string password = "password";


        public async Task<bool> RegisterContract(string states)
        {
            string code = this.GetContractCode(states);

            Contract contract = new Contract(this.senderAddress, this.password);

            contract.Compile(code);
            await contract.Deploy();

            // setting the static variables
            var contractAddress = contract.ContractAddress;
            var abi = contract.Abi;
            var eth = @"var p = eth.contract(" + abi + @").at(""" + contractAddress + @""")";

            ProfileContract.ContractAddress = contractAddress;
            ProfileContract.Abi = abi;

            return true;
        }

        public async Task Intantiate(string state, string formCode, string comments, string approver)
        {
            Contract contract = new Contract(this.senderAddress, this.password);
            await contract.SendTransaction(ProfileContract.Abi, ProfileContract.ContractAddress, state, formCode, comments, approver);
        }

        private string GetContractCode(string states)
        {
            string[] arr = states.Split(',');

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"
pragma solidity ^0.4.11;

contract Profile {

    struct Instance {
        string FormCode;
        string Comments;
        string Approver;
    }

    mapping(string => Instance) instances;       
            ");

            foreach (string str in arr)
            {
                sb.AppendLine(@"

    function " + str.Trim() + @"(string FormCode, string Comments, string Approver) public {
        instances[FormCode] = Instance({ FormCode: FormCode, Comments: Comments,  Approver: Approver});
    }
");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}
