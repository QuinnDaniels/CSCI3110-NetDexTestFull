"use strict";

console.info("PAGE: socialMediaEdit.js reached!");
import { SocialMediaService } from "./socialMediaHandler.js";
import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";
const personRepo = new PersonRepository("https://localhost:7134/api/people");
//const SocialMediaService = new SocialMediaService("https://localhost:7134/api/socialmedia");

/*
window.onload = function () {
    document.getElementById("socialmediaHeading").onclick = function fun() {
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
const _socialmedia = document.getElementById("_socialmedia").value;
console.log(_socialmedia);

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


    const editHeading = document.getElementById("socialmediaHeading");
    console.log("checking editheading from main thread:", editHeading);
    
    DOM.removeChildren(editHeading);
    editHeading.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));
    

    await DOMLoadFunction();
        
    
    
    async function populateSocialmediaData() {
        console.log("DOM populateSocialmediaData reached");
        try {
            const _socialmedia = document.getElementById("_socialmedia").value;
            console.log("populateSocialmediaData(): _socialmedia: ", _socialmedia);

            const item = await SocialMediaService.getOne(_socialmedia);
            console.log("populateSocialmediaData(): item: ", item);
            
            
            DOM.setElementText("#_oldCategoryField", item.categoryField);
            //DOM.setElementText("#_oldSocialHandle", item.socialHandle);
            DOM.setElementText("#_CategoryField",    item.categoryField);
            DOM.setElementText("#_SocialHandle",    item.socialHandle);
            
            
            const editHeading = document.getElementById("socialmediaHeading");
            console.log("heading: ",editHeading);
            DOM.removeChildren(editHeading);
            //console.log("heading: removed children: head: ",editHeading);
            //console.log("heading: removed children: item: ",item);
            
            //DOM.createImg("/images/ajax-loader.gif", "Loading image"));

            editHeading.appendChild(document.createTextNode(`Editing Socialmedia #_${_socialmedia} (global)`));

            
        }
        catch (error) {
            console.log(error);
            //window.location.replace(`/dex/u/${email}/p/${numId}/cont/soc/${_socialmedia}`);
        }}
        


        window.addEventListener("DOMContentLoaded", async ()  => {
            console.log("DOM ContentLoaded reached");
        });
        
    async function DOMLoadFunction() {
        console.log("DOM populateSocialmediaData reached");
        const updateSocialmediaForm = document.getElementById("editSocialmediaForm");
        
        const form = updateSocialmediaForm;
        const record =  document.getElementById("_record").value;
        const socialmedia =  document.getElementById("_socialmedia").value;
        const CategoryField =  document.getElementById("_CategoryField").value;
        const SocialHandle =  document.getElementById("_SocialHandle").value;
        await populateSocialmediaData();
        //console.log("recordsId:", recordsId);
        
        
        form.addEventListener("submit", async (e) => {
            e.preventDefault();
            console.log("DOM submit event reached");
            const form = e.target;
            console.log("form: ", form);
            const formData = new FormData(updateSocialmediaForm);
            //const formData = new FormData(form);
            for (const [key, value] of formData.entries()) {        // 3. Loop through and output the form data to the console
                console.log(key, value);
            }


            try {
                const result = await SocialMediaService.update(updateSocialmediaForm);
                console.log("Result: ", result);
                alert(`Socialmedia updated successfully! Redirecting...`);// \nResult: ${result}\n`);
                //window.location.replace(`/dex/u/${email}/p/${numId}/cont/soc/${_socialmedia}`);
                window.location.replace(`/dex/u/${email}/p/${numId}/cont/list/soc`);

                //const personId = form.PersonId.value;
                //const userEmail = form.ApplicationUserEmail.value;
                //window.location.href = `/dex/u/${userEmail}/p/${numId}/cont/soc/${socialmediaId}`;
                //window.location.replace(`/dex/u/${currentEmail}`);

            } catch (err) {
                console.error("Edit error:", err);
                alert("Failed to update socialmedia.");
            }
        });
    }
    




    console.log("DOM ContentLoaded passed");

