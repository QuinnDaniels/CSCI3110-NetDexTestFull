"use strict";

console.info("PAGE: entryItemEdit.js reached!");
import { EntryItemService } from "./entryItemHandler.js";
import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";
const personRepo = new PersonRepository("https://localhost:7134/api/people");
//const EntryItemService = new EntryItemService("https://localhost:7134/api/entry");

/*
window.onload = function () {
    document.getElementById("entryHeading").onclick = function fun() {
        alert("hello");
        // tellMeForm();
        //validation code to see State field is mandatory.  
    
    }
}
*/


const email = document.getElementById("email").value;
console.log(email);
const lastperson = document.getElementById("lastPerson").value;
console.log(lastperson);
const _entry = document.getElementById("_entry").value;
console.log(_entry);

var numId = 0;

try {
    const personLocal = await personRepo.read(email, lastperson);
    console.log(personLocal);
    numId = personLocal.localCounter;
    console.log(personLocal);
    console.info("localcounter", numId);
    
    //let numId = personLocal.localCounter;
} catch (error) {
    //console.info("applicationUserEmail", localCounter);
    console.warn("error fetching person for LocalCount. links may not work.  ");
    console.info("localcounter", numId);
}


    const editHeading = document.getElementById("entryHeading");
    console.log("checking editheading from main thread:", editHeading);
    
    DOM.removeChildren(editHeading);
    editHeading.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));
    

    await DOMLoadFunction();
        
    
    
    async function populateEntryData() {
        console.log("DOM populateEntryData reached");
        try {
            const _entry = document.getElementById("_entry").value;
            console.log("populateEntryData(): _entry: ", _entry);

            const item = await EntryItemService.getOne(_entry);
            console.log("populateEntryData(): item: ", item);
            
            
            DOM.setElementText("#_oldShortTitle", item.shortTitle);
            //DOM.setElementText("#_oldFlavorText", item.flavorText);
            DOM.setElementText("#_ShortTitle",    item.shortTitle);
            DOM.setElementText("#_FlavorText",    item.flavorText);
            
            
            const editHeading = document.getElementById("entryHeading");
            console.log("heading: ",editHeading);
            DOM.removeChildren(editHeading);
            //console.log("heading: removed children: head: ",editHeading);
            //console.log("heading: removed children: item: ",item);
            
            //DOM.createImg("/images/ajax-loader.gif", "Loading image"));

            editHeading.appendChild(document.createTextNode(`Editing Entry #_${_entry} (global)`));

            
        }
        catch (error) {
            console.log(error);
            //window.location.replace(`/dex/u/${email}/p/${numId}/rec/ie/${_entry}`);
        }}
        


        window.addEventListener("DOMContentLoaded", async ()  => {
            console.log("DOM ContentLoaded reached");
        });
        
    async function DOMLoadFunction() {
        console.log("DOM populateEntryData reached");
        const updateEntryForm = document.getElementById("editEntryForm");
        
        const form = updateEntryForm;
        const record =  document.getElementById("_record").value;
        const entry =  document.getElementById("_entry").value;
        const ShortTitle =  document.getElementById("_ShortTitle").value;
        const FlavorText =  document.getElementById("_FlavorText").value;
        await populateEntryData();
        //console.log("recordsId:", recordsId);
        
        
        form.addEventListener("submit", async (e) => {
            e.preventDefault();
            console.log("DOM submit event reached");
            const form = e.target;
            console.log("form: ", form);
            const formData = new FormData(updateEntryForm);
            //const formData = new FormData(form);
            for (const [key, value] of formData.entries()) {        // 3. Loop through and output the form data to the console
                console.log(key, value);
            }


            try {
                const result = await EntryItemService.update(updateEntryForm);
                console.log("Result: ", result);
                alert(`Entry updated successfully! Redirecting...`);// \nResult: ${result}\n`);
                //window.location.replace(`/dex/u/${email}/p/${numId}/rec/ie/${_entry}`);
                window.location.replace(`/dex/u/${email}/p/${numId}/rec/list/ie`);

                //const personId = form.PersonId.value;
                //const userEmail = form.ApplicationUserEmail.value;
                //window.location.href = `/dex/u/${userEmail}/p/${numId}/rec/ie/${entryId}`;
                //window.location.replace(`/dex/u/${currentEmail}`);

            } catch (err) {
                console.error("Edit error:", err);
                alert("Failed to update entry.");
            }
        });
    }
    




    console.log("DOM ContentLoaded passed");

