"use strict";

console.log("\npersonCreate.js loaded!\n");

import { PersonRepository } from "./DexHolderRepository.js";

const personRepo = new PersonRepository("https://localhost:7134/api/people");


const email = document.getElementById("email").value;
DOM.logElementToConsole(email);
//const dexResponse = await userRepo.readDex(email);
//console.log("LIST dexResponse: ", dexResponse);
//console.log("LIST people: ", dexResponse.People);
//const PeopleArray = dexResponse.People;




const personCreateForm = document.getElementById("formCreatePerson");
const headOfPerson = document.getElementById("headOfPerson");
function formAlert(stringer) {
    let string = stringer;
    alert(string);
}
function tellMeForm() {
    const formData = new FormData(personCreateForm);     // 2. Get the form data
    let stringer = ""
    for (const [key, value] of formData.entries()) {        // 3. Loop through and output the form data to the console
        console.log(key, value);
        stringer += `\n\t${key} = ${value}`
    }
    formAlert(stringer);
    //for (let i = 0; i < dataArray.length; i++) {
    //    stringBuilder += dataArray[i];
    //    if (i < dataArray.length - 1) {
    //        stringBuilder += '&'; // Add '&' between parameters
    //    }}

}




window.onload = function () {
    document.getElementById("headOfPerson").onclick = function fun() {
        alert("hello");
        tellMeForm();
        //validation code to see State field is mandatory.  
    }
}


console.log("\nclick listener passed!\n");

 personCreateForm.addEventListener("submit", async (event) => {
     event.preventDefault();
     console.log("\n\n\n BUTTON CLICK: Attempting to create a person!\n\n");
     console.log("personCreateForm", personCreateForm);
     try {
        const form = event.target;
        console.log("form: ", form);
        const formData = new FormData(form); //  constructs form-encoded payload
         for (const [key, value] of formData.entries()) {        // 3. Loop through and output the form data to the console
             console.log(key, value);
         }

         const result = await personRepo.create(formData);
         console.log("Result: ", result);
         window.location.replace(`/dex/u/${email}`);
     } catch (ex) {
        console.log("Exception: ", ex);
     }
 });












//personCreateForm.addEventListener("submit", async (e) => {
//        e.preventDefault();

//        const form = e.target;
//        const formData = new FormData(form); //  constructs form-encoded payload

//        try {
//            const response = await fetch("https://localhost:7134/api/people/forms/create", {
//                method: "POST",
//                body: formData //  no Content-Type header needed
//            });

//            if (!response.ok) throw new Error("There was an HTTP error creating the person data.");
//            const result = await response.json();
//            console.log("Success:", result);
//        } catch (error) {
//            console.error("Error:", error);
//        }
//});






//personCreateForm.addEventListener("submit", async (event) => {
//    event.preventDefault();

//    const form = event.target;
//    const formData = new FormData(form); // ✅ constructs form-encoded payload

//    try {
//        const response = await fetch("https://localhost:7134/api/people/forms/create", {
//            method: "POST",
//            body: formData // ✅ no Content-Type header needed
//        });

//        if (!response.ok) throw new Error("There was an HTTP error creating the pet data.");
//        const result = await response.json();
//        console.log("Success:", result);
//        console.log("Result: ", result);
//        //window.location.replace("/dex/list");

//    } catch (error) {
//        console.error("Error:", error);
//    }
//});