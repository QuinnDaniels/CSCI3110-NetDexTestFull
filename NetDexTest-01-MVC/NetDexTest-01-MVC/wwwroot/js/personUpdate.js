"use strict";

import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";

const personRepo = new PersonRepository("https://localhost:7134/api/people");

const personHeading = document.getElementById("personHeading");
DOM.removeChildren(personHeading);
personHeading.appendChild(
    DOM.createImg("/images/ajax-loader.gif", "Loading image"));

const personId = document.getElementById("_Id").value;
console.log("personId:", personId);

const currentEmail = document.getElementById("_Email").value;
const localCounter = document.getElementById("_LocalCounter").value;
console.log("localCounter:", personId);


await populatePersonData();
const updatePersonForm = document.getElementById("updatePersonForm");



updatePersonForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const formData = new FormData(updatePersonForm);
    try {
        await personRepo.update(formData, personId);
        window.location.replace(`/dex/u/${currentEmail}`);
    }
    catch (error) {
        console.log(error);
    }
});

async function populatePersonData() {
    try {
        const person = await personRepo.read(currentEmail, localCounter);
        console.log(person);


        DOM.setElementValue("#tmpEmail", currentEmail);
        DOM.setElementValue("#tmpId", person.id);
        DOM.setElementValue("#tmpNickname", person.nickname);
        DOM.setElementValue("#tmpNameFirst", person.nameFirst);
        DOM.setElementValue("#tmpPhNameFirst", person.phNameFirst);
        DOM.setElementValue("#tmpNameMiddle", person.nameMiddle);
        DOM.setElementValue("#tmpPhNameMiddle", person.phNameMiddle);
        DOM.setElementValue("#tmpNameLast", person.nameLast);
        DOM.setElementValue("#tmpPhNameLast", person.phNameLast);
        DOM.setElementValue("#tmpDateOfBirth", person.DateOfBirth);
        DOM.setElementValue("#tmpGender", person.gender);
        DOM.setElementValue("#tmpPronouns", person.pronouns);
        DOM.setElementValue("#tmpRating", person.rating);
        DOM.setElementValue("#tmpFavorite", person.favorite);


        DOM.removeChildren(personHeading);
        personHeading.appendChild(document.createTextNode(`${localCounter} - ${person.nickname}`));

        
        
        
    }
    catch (error) {
        console.log(error);
        window.location.replace(`/dex/u/${currentEmail}`);
    }
}
console.log("form: ", updatePersonForm);
