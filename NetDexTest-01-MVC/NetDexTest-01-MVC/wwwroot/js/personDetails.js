"use strict";
import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";

const personRepo = new PersonRepository("https://localhost:7134/api/people");
const email = document.getElementById("email").value;
const lastperson = document.getElementById("lastPerson").value;

//const urlSections = window.location.href.split("/");
//const petId = urlSections[5];



try {
    const person = await personRepo.read(email, lastperson);
    console.log(person);

    DOM.setElementText("#personId", person.Id);
    DOM.setElementText("#personAppUsername", person.AppUsername);
    DOM.setElementText("#personAppEmail", person.AppEmail);
    DOM.setElementText("#personLocalCounter", person.LocalCounter);
    DOM.setElementText("#personDexId", person.DexId);
    DOM.setElementText("#personNickname", person.Nickname);
    DOM.setElementText("#personNameFirst", person.NameFirst);
    DOM.setElementText("#personNameMiddle", person.NameMiddle);
    DOM.setElementText("#personNameLast", person.NameLast);
    DOM.setElementText("#personPhNameFirst", person.PhNameFirst);
    DOM.setElementText("#personPhNameMiddle", person.PhNameMiddle);
    DOM.setElementText("#personPhNameLast", person.PhNameLast);
    DOM.setElementText("#personDateOfBirth", person.DateOfBirth);//  DOM.formatDateMMDDYYYY(person.DateOfBirth));
    DOM.setElementText("#personGender", person.Gender);
    DOM.setElementText("#personPronouns", person.Pronouns);
    DOM.setElementText("#personRating", person.Rating);
    DOM.setElementText("#personFavorite", person.Favorite);
    DOM.setElementText("#personRcEntryItemsCount", person.RcEntryItemsCount);
    DOM.setElementText("#personCiSocialMediasCount", person.CiSocialMediasCount);
    DOM.setElementText("#personPersonParentsCount", person.PersonParentsCount);
    DOM.setElementText("#personPersonChildrenCount", person.PersonChildrenCount);


}
catch (error) {
    console.log(error);
    window.location.replace(`/dex/u/${email}/p/${lastperson}`);
}
