"use strict";

import { DexHolderRepository } from "./DexHolderRepository.js";
import { DOM } from "./DOMCreator.js";

const dexRepo = new DexHolderRepository("https://localhost:7134/api/user");

const personHeading = document.getElementById("personHeading");
DOM.removeChildren(personHeading);
personHeading.appendChild(
    DOM.createImg("/images/ajax-loader.gif", "Loading image"));


const userId = document.getElementById("_ApplicationUserId").value;
console.log("userId:", userId);

const currentEmail = document.getElementById("_ApplicationEmail").value;
const dexNumber = document.getElementById("_DexId").value;
console.log("dexNumber:", dexNumber);


await populatePersonData();
const updatePersonForm = document.getElementById("updateDexForm");
updatePersonForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const formData = new FormData(updatePersonForm);
    try {
        await dexRepo.update(formData, userId);
        window.location.replace(`/dex/u/${currentEmail}`);
    }
    catch (error) {
        console.log(error);
    }
});

async function populatePersonData() {
    try {
        const person = await dexRepo.read(userId); //currentEmail
        console.log(person);


        DOM.setElementValue("#tmpApplicationUserId", userId);
        DOM.setElementValue("#tmpApplicationUserName", person.ApplicationUserName);
        DOM.setElementValue("#tmpApplicationEmail", person.ApplicationEmail);
        DOM.setElementValue("#tmpFirstName", person.FirstName);
        DOM.setElementValue("#tmpMiddleName", person.MiddleName);
        DOM.setElementValue("#tmpLastName", person.LastName);
        DOM.setElementValue("#tmpGender", person.Gender);
        DOM.setElementValue("#tmpPronouns", person.Pronouns);
        DOM.setElementValue("#tmpDateOfBirth", person.DateOfBirth);
        DOM.setElementValue("#tmpDexId", person.DexId);


        DOM.removeChildren(personHeading);
        personHeading.appendChild(document.createTextNode(`${dexNumber} - ${person.ApplicationUserName}`));
    }
    catch (error) {
        console.log(error);
        window.location.replace(`/dex/u/${currentEmail}`);
    }
}
