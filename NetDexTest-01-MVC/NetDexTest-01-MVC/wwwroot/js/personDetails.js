"use strict";
import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";

const personRepo = new PersonRepository("https://localhost:7134/api/people");
const email = document.getElementById("email").value;
const lastperson = document.getElementById("lastPerson").value;
console.log("lastperson:", lastperson);
console.log("email:", email);

//const urlSections = window.location.href.split("/");
//const petId = urlSections[5];
const personHeading = document.getElementById("personHeading");
try {
    DOM.removeChildren(personHeading);
    personHeading.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));

} catch (e) {
    console.log("heading error:", e);


} 



try {
    //const person = await personRepo.read(email, lastperson);
    //console.log(person);
    await populatePersonData();


    //DOM.personDetailsButtons("redirectLinkHolder", email, person.Nickname);


}
catch (error) {
    console.log("trycatch1:", error);
    //window.location.replace(`/dex/u/${email}/p/${lastperson}`);
}

async function populatePersonData() {
    try {
        const person = await personRepo.read(email, lastperson);
        console.log(person);


        DOM.setElementText("#personId", person.id);
        DOM.setElementText("#personAppUsername", person.appUsername);
        DOM.setElementText("#personAppEmail", person.appEmail);
        DOM.setElementText("#personLocalCounter", person.localCounter);
        DOM.setElementText("#personDexId", person.dexId);
        DOM.setElementText("#personNickname", person.nickname);
        DOM.setElementText("#personNameFirst", person.nameFirst);
        DOM.setElementText("#personNameMiddle", person.nameMiddle);
        DOM.setElementText("#personNameLast", person.nameLast);
        DOM.setElementText("#personPhNameFirst", person.phNameFirst);
        DOM.setElementText("#personPhNameMiddle", person.phNameMiddle);
        DOM.setElementText("#personPhNameLast", person.phNameLast);
        DOM.setElementText("#personDateOfBirth", person.dateOfBirth);//  DOM.formatDateMMDDYYYY(person.DateOfBirth));
        DOM.setElementText("#personGender", person.gender);
        DOM.setElementText("#personPronouns", person.pronouns);
        DOM.setElementText("#personRating", person.rating);
        DOM.setElementText("#personFavorite", person.favorite);
        DOM.setElementText("#personRcEntryItemsCount", person.rcEntryItemsCount);
        DOM.setElementText("#personCiSocialMediasCount", person.ciSocialMediasCount);
        DOM.setElementText("#personPersonParentsCount", person.personParentsCount);
        DOM.setElementText("#personPersonChildrenCount", person.personChildrenCount);

        const links = document.getElementById("redirectLinkHolder");
        links.appendChild(DOM.personDetailsButtons(email, lastperson));


        DOM.removeChildren(personHeading);


        personHeading.appendChild(document.createTextNode(`${localCounter} - ${person.nickname}`));
    }
    catch (error) {
        console.log(error);
        window.location.replace(`/dex/u/${currentEmail}`);
    }
}
