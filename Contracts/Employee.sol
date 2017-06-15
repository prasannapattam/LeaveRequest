pragma solidity ^0.4.11;

contract Employee {

    struct Person {
        uint id;
        string name;
        uint managerId;
    }

    mapping(uint => Person) persons;

    function Employee() {
        
    }

    function addEmployee(uint id, string name) public {
        persons[id] = Person({ id: id, name: name, managerId: 0 });
    }

    function getEmployee(uint id) public constant returns(uint, string, uint) {
        return (persons[id].id, persons[id].name, persons[id].managerId);
    }
}
