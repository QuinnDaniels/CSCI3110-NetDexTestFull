"use strict";

//fetch('{URL string}', { options })
//    .then({ callback function})   // Extract the data from the response
//    .then({ callback function})   // Do something with the data
//    .catch({ callback function}); // Handle any errors


function loadingText() {

    //// Create Text <td>
    const tr1 = document.createElement("tr");
    document.querySelector("#tbodyBoardGameTable").appendChild(tr1);

    const td1 = document.createElement("td");
    td1.appendChild(document.createTextNode('loading...'));
    tr1.appendChild(td1);
}


////CHECKPOINT 2: send a request to the external WebAPI application to get all board games

async function fetchData() {

    console.log('Checkpoint 2: using fetch().then().then().catch() to get all boardgames')
    const response = fetch('https://localhost:7134/api/user/admin/all')       // use the full URL from the other RUNNING application
        .then(response => response.json()) // Get the json from the response
        .then(data => console.log('Checkpoint 2 Data: ', data)) // Log the data to the console
        .catch(error => console.error('Error:', error)); // Log the error to the error console
}

fetchData();


//CHECKPOINT 3
async function fetchDataWithTryCatch() {
    console.log('Checkpoint 3: turn fetch().then().then().catch() into await fetch() using a try-catch block');
    try {
        const response = await fetch('https://localhost:7134/api/user/admin/all');

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        console.log('Checkpoint 3 Data: ', data);
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

fetchDataWithTryCatch();


//CHECKPOINT 4
import { UserRepository } from "./UserRepository.js"; // import repository

const userRepo = new UserRepository("https://localhost:7134/api/user"); // instantiate repository, set base address

// add readAll() method to class BoardGameRepository

async function fetchDataFromClass() {
    console.log('Checkpoint 4: using method readAll() from class UserRepository');
    try {
        let data = await userRepo.readAll();

        console.log('Checkpoint 4 Data: ', data);
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

fetchDataFromClass();



import { DOM } from "./DOMCreator.js"; // import repository

function addUserToTable(tbody, AdminUserVM) {
    const tr = document.createElement("tr");
    tbody.appendChild(tr);
    
    //// Create Text <td>
    //    const td = document.createElement("td");
    //    td.appendChild(document.createTextNode(boardGame.id));
    //    tr.appendChild(td);

    tr.appendChild(DOM.createTextTD(AdminUserVM.DexHolderId));
    tr.appendChild(DOM.createTextTD(AdminUserVM.UserName));
    tr.appendChild(DOM.createULfromArrayPropertyTD(AdminUserVM.Roles, "RoleName"));
    tr.appendChild(DOM.createTextTD(AdminUserVM.Email));
    tr.appendChild(DOM.createHiddenTextTD(AdminUserVM.Id));
    tr.appendChild(DOM.createTextTD(AdminUserVM.FirstName));
    tr.appendChild(DOM.createTextTD(AdminUserVM.LastName));
    tr.appendChild(DOM.createTextTD(AdminUserVM.PeopleCount));
    tr.appendChild(DOM.createTextTD(AdminUserVM.AccessFailedCount));

    //const uRoles = AdminUserVM.Roles.filter(user)


    //Array.from(AdminUserVM.Roles)
    //    .forEach(role => {
    //        tr.appendChild(DOM.createTextTD(role.RoleName));
    //    });


    // CHECKPOINT 8
    //tr.appendChild(createTDWithLinks(AdminUserVM.id)) // Function creates [Edit | Details | Delete] links

    return tr;
}


//const userTableBody = document.querySelector("#tbodyAdminUserVMTable"); // read the <tbody> from the DOM/document
//let item = await userRepo.readOneId(1);



//const check6 = addUserToTable(userTableBody, item)
//console.log('CHECKPOINT 6: ', check6);



//DOM.removeChildren(userTableBody);
//loadingText();




async function populateUsers(userRepo) {
    let tbody = document.querySelector("#tbodyAdminUserVMTable");
    try {
        let users = await userRepo.readAll();
        //users.sort();
        let sortedusers = users.toSorted(function (a, b) { return a.DexHolderId - b.DexHolderId });

        try { DOM.removeChildren(tbody); }
        catch (error) { console.log('Remove Children Error: ', error); }

        sortedusers.forEach((AdminUserVM) => {
            console.log('CHECKPOINT 7: ',
                tbody.appendChild(
                    addUserToTable(tbody, AdminUserVM)
                ))
        });
    }
    catch (error) {
        console.error('Fetch error:', error);
    }
}

DOM.removeChildren(document.querySelector("#tbodyAdminUserVMTable"));
await populateUsers(userRepo);


// CHECKPOINT 8:
function createTDWithLinks(id) {
    const td = document.createElement("td");
    td.appendChild(DOM.createButtonLink("Edit", `/boardgame/edit/${id}`, 'warning'));
    td.appendChild(document.createTextNode(" | "));
    td.appendChild(DOM.createButtonLink("Details", `/boardgame/details/${id}`, 'info'));
    td.appendChild(document.createTextNode(" | "));
    td.appendChild(DOM.createButtonLink("Delete", `/boardgame/delete/${id}`, 'danger'));
    return td;
}


// TODO : sort the users by DexHolderId after recieving
//.sort(function(a, b){return a.year - b.year});
// https://www.w3schools.com/js/js_array_sort.asp