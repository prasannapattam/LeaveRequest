
pragma solidity ^0.4.11;

contract Profile {

    struct Instance {
        string FormCode;
        string Comments;
        string Approver;
    }

    mapping(string => Instance) instances;       
            


    function Create(string FormCode, string Comments, string Approver) public {
        instances[FormCode] = Instance({ FormCode: FormCode, Comments: Comments,  Approver: Approver});
    }



    function SubmitToManager(string FormCode, string Comments, string Approver) public {
        instances[FormCode] = Instance({ FormCode: FormCode, Comments: Comments,  Approver: Approver});
    }



    function SubmitToDirector(string FormCode, string Comments, string Approver) public {
        instances[FormCode] = Instance({ FormCode: FormCode, Comments: Comments,  Approver: Approver});
    }



    function Complete(string FormCode, string Comments, string Approver) public {
        instances[FormCode] = Instance({ FormCode: FormCode, Comments: Comments,  Approver: Approver});
    }

}
