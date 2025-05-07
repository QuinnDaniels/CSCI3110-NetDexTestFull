"use strict";

console.info("PAGE: entryItemEdit.js reached!");
import { EntryItemService } from "./entryItemHandler.js";
import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";
const personRepo = new PersonRepository("https://localhost:7134/api/people");
const entryRepo = new EntryItemService("https://localhost:7134/api/entry");


const email = document.getElementById("email").value;
const lastperson = document.getElementById("lastPerson").value;
var numId = 0;

try {
    const personLocal = await personRepo.read(email, lastperson);
    console.log(personLocal);
    numId = personLocal.localCounter;
    console.log(personLocal);
    console.info("localcounter", numId);
    
    //let numId = personLocal.localCounter;
} catch (error) {
    console.warn("error fetching person for LocalCount. links may not work.  ");
    console.info("localcounter", numId);

    
}


window.addEventListener("DOMContentLoaded", async ()  => {
    
    const editHeading = document.getElementById("editEntryForm");
    DOM.removeChildren(editHeading);
    editHeading.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));
    
        


    const updateEntryForm = document.getElementById("editEntryForm");
    
    await populateEntryData();
    const form = updateEntryForm;
    const record =  document.getElementById("_record").value;
    const entry =  document.getElementById("_entry").value;
    const ShortTitle =  document.getElementById("_ShortTitle").value;
    const FlavorText =  document.getElementById("_FlavorText").value;
    console.log("recordsId:", recordsId);
    
    
    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const formData = new FormData(updateEntryForm);
        
        try {
            await entryRepo.update(formData);
            alert("Entry updated successfully.");
            
            //const personId = form.PersonId.value;
            //const userEmail = form.ApplicationUserEmail.value;
            const entryId = form.Id.value;
            const shortTitle = form.ShortTitle.value;  //document.getElementById("_ShortTitle").value;
            const flavorText = form.FlavorText.value;  //document.getElementById("_FlavorText").value;
            const recordCollector =  form.RecordCollectorId.value;  //document.getElementById("_record").value;
            window.location.href = `/dex/u/${userEmail}/p/${numId}/rec/ie/${entryId}`;
            //window.location.replace(`/dex/u/${currentEmail}`);

        } catch (err) {
            console.error("Edit error:", err);
            alert("Failed to update entry.");
        }
    });


    async function populateEntryData() {
        try {
            const item = await entryRepo.getOne(entry);
            console.log(item);
    

            DOM.setElementText("#_oldShortTitle", item.shortTitle);
            DOM.setElementText("#_oldFlavorText", item.flavorText);
            DOM.setElementText("#_ShortTitle",    item.shortTitle);
            DOM.setElementText("#_FlavorText",    item.flavorText);
    
    
            DOM.removeChildren(editEntryFormHeading);
            editHeading.appendChild(document.createTextNode(`Entries for`));
        }
        catch (error) {
            console.log(error);
            window.location.replace(`/dex/u/${currentEmail}/p/${numId}/rec/ie/${entryId}`);
        }
    


    }
});