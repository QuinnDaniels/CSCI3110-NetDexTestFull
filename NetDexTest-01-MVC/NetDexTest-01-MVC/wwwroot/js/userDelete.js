"use strict";

console.log("userDelete.js reached");
import { DexHolderRepository } from "./DexHolderRepository.js";
import { DOM } from "./DOMCreator.js";

const dexRepo = new DexHolderRepository("https://localhost:7134/api/user");

const personHeading = document.querySelector("#personHeading");
DOM.removeChildren(personHeading);
personHeading.appendChild(document.createTextNode("Loading..."));
personHeading.appendChild(
    DOM.createImg("/images/ajax-loader.gif", "Loading image"));


const dexId = document.getElementById("_dId").value;
const userId = document.getElementById("_aId").value;
console.log("userId:", userId);


const currentEmail = document.getElementById("_Email").value;
const username = document.getElementById("_UserName").value;
console.log("currentEmail:", currentEmail);
console.log("dexId:", dexId);
//console.log("lastperson:", lastperson);

//await populatePersonData();
const formUserDelete = document.getElementById("formUserDelete");


try {
    //const person = await dexRepo.read(email, lastperson);
    await populateLinkOnForm(formUserDelete);
    console.log("populateLinkOnForm passed");


    //DOM.personDetailsButtons("redirectLinkHolder", email, person.Nickname);


}
catch (error) {
    console.log("trycatch1:", error);
    //window.location.replace(`/dex/u/${email}/p/${lastperson}`);
}


formUserDelete.addEventListener("submit", async (e) => {
    e.preventDefault();
    const formData = new FormData(formUserDelete);

    try {
        await dexRepo.deletePerson(currentEmail);
        //await dexRepo.deletePerson(currentEmail, lastperson );
        //await dexRepo.deletePerson(currentEmail, dexId );
        ////await dexRepo.deletePerson(formData.get("id"));
        //window.location.replace(`/dex/u/${currentEmail}`);
        window.location.replace(`/auth/login`);
    }
    catch (error) {
        console.log(error);
    }
});




//async function populatePersonData() {
//    try {
//        const person = await dexRepo.read(email, lastperson);
//        console.log(person);


//        DOM.setElementText("#userId", person.id);
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


async function populateLinkOnForm(formUserDelete) {
    try {
        //const person = await dexRepo.read(currentEmail, lastperson);
        //console.log(person);
        console.log(currentEmail);
        console.log(dexId);
        console.log(username);



        const link = document.createElement('div');
        link.innerHTML =
            `<a class="btn btn-secondary" href="/dex/u/${currentEmail}/">Cancel</a>`;

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

        formUserDelete.appendChild(link);

        DOM.removeChildren(personHeading);


        personHeading.appendChild(document.createTextNode(`${dexId} - ${username}`));
        //personHeading.appendChild(document.createTextNode(`${localCounter} - ${person.nickname}`));
    }
    catch (error) {
        console.log(error);
        //window.location.replace(`/dex/u/${currentEmail}`);
    }
}
