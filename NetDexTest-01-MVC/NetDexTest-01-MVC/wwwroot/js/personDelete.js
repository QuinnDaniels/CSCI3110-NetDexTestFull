"use strict";

console.log("userDelete.js reached");
import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";

const personRepo = new PersonRepository("https://localhost:7134/api/people");

const personHeading = document.querySelector("#personHeading");
DOM.removeChildren(personHeading);
personHeading.appendChild(document.createTextNode("Loading..."));
personHeading.appendChild(
    DOM.createImg("/images/ajax-loader.gif", "Loading image"));


const personId = document.getElementById("_Id").value;
console.log("personId:", personId);


const currentEmail = document.getElementById("_Email").value;
const localCounter = document.getElementById("_Lc").value;
const lastperson = document.getElementById("_Nickname").value;
console.log("currentEmail:", currentEmail);
console.log("localCounter:", localCounter);
console.log("lastperson:", lastperson);

//await populatePersonData();
const formPersonDelete = document.getElementById("formPersonDelete");


try {
    //const person = await personRepo.read(email, lastperson);
    await populateLinkOnForm(formPersonDelete);
    console.log("populateLinkOnForm passed");


    //DOM.personDetailsButtons("redirectLinkHolder", email, person.Nickname);


}
catch (error) {
    console.log("trycatch1:", error);
    //window.location.replace(`/dex/u/${email}/p/${lastperson}`);
}


formPersonDelete.addEventListener("submit", async (e) => {
    e.preventDefault();
    const formData = new FormData(formPersonDelete);

    try {
        await personRepo.deletePerson(currentEmail, lastperson);
        //await personRepo.deletePerson(currentEmail, localCounter );
        ////await personRepo.deletePerson(formData.get("id"));
        window.location.replace(`/dex/u/${currentEmail}`);
    }
    catch (error) {
        console.log(error);
    }
});




//async function populatePersonData() {
//    try {
//        const person = await personRepo.read(email, lastperson);
//        console.log(person);


//        DOM.setElementText("#personId", person.id);
//        DOM.setElementText("#personAppUsername", person.appUsername);
//        DOM.setElementText("#personAppEmail", person.appEmail);
//        DOM.setElementText("#personLocalCounter", person.localCounter);
//        DOM.setElementText("#personDexId", person.dexId);
//        DOM.setElementText("#personNickname", person.nickname);
//        DOM.setElementText("#personNameFirst", person.nameFirst);
//        DOM.setElementText("#personNameMiddle", person.nameMiddle);
//        DOM.setElementText("#personNameLast", person.nameLast);
//        DOM.setElementText("#personPhNameFirst", person.phNameFirst);
//        DOM.setElementText("#personPhNameMiddle", person.phNameMiddle);
//        DOM.setElementText("#personPhNameLast", person.phNameLast);
//        DOM.setElementText("#personDateOfBirth", person.dateOfBirth);//  DOM.formatDateMMDDYYYY(person.DateOfBirth));
//        DOM.setElementText("#personGender", person.gender);
//        DOM.setElementText("#personPronouns", person.pronouns);
//        DOM.setElementText("#personRating", person.rating);
//        DOM.setElementText("#personFavorite", person.favorite);
//        DOM.setElementText("#personRcEntryItemsCount", person.rcEntryItemsCount);
//        DOM.setElementText("#personCiSocialMediasCount", person.ciSocialMediasCount);
//        DOM.setElementText("#personPersonParentsCount", person.personParentsCount);
//        DOM.setElementText("#personPersonChildrenCount", person.personChildrenCount);

//        const links = document.getElementById("redirectLinkHolder");
//        links.appendChild(DOM.personDetailsButtons(email, lastperson));


//        DOM.removeChildren(personHeading);


//        personHeading.appendChild(document.createTextNode(`${localCounter} - ${person.nickname}`));
//    }
//    catch (error) {
//        console.log(error);
//        window.location.replace(`/dex/u/${currentEmail}`);
//    }
//}


async function populateLinkOnForm(formPersonDelete) {
    try {
        //const person = await personRepo.read(currentEmail, lastperson);
        //console.log(person);
        console.log(currentEmail);
        console.log(lastperson);



        const link = document.createElement('div');
        link.innerHTML =
            `<a class="btn btn-secondary" href="/dex/u/${currentEmail}/p/${lastperson}">Cancel</a>`;

        link.style.float = 'inline-start';
        //link.classList.add("btn");
        //link.classList.add("btn-secondary");
        //link.href = `/dex/u/${currentEmail}/p/${lastperson}`; // HACK ideally i should check if a user exists first so im not linking to a ghost page
        //////link.innerText("Cancel");
        ///*
        //TypeError: link.textContent is not a function
        //    at populateLinkOnForm (personDelete.js:119:14)
        //    at personDelete.js:31:11
        //*/



        console.log(link);

        formPersonDelete.appendChild(link);

        DOM.removeChildren(personHeading);


        personHeading.appendChild(document.createTextNode(`${localCounter} - ${lastperson}`));
        //personHeading.appendChild(document.createTextNode(`${localCounter} - ${person.nickname}`));
    }
    catch (error) {
        console.log(error);
        //window.location.replace(`/dex/u/${currentEmail}`);
    }
}
