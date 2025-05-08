"use strict";

import { SocialMediaService } from "./socialMediaHandler.js";
import { DOM } from "./DOMCreator.js";

window.addEventListener("DOMContentLoaded", async () => {
    const form = document.getElementById("deleteSocialmediaForm");

    const item = await returnSocialmediaData();
    console.log("populateSocialmediaData(): item: ", item);

    await populateSocialmediaData(item);
    const headContainer = document.getElementById("headContainer");

    const socialmediaHeading = document.getElementById("socialmediaHead");
    DOM.removeChildren(headContainer);
    headContainer.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));

    const localCounter = item.localCounter;

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const counter = localCounter;

        const socialmediaId = document.getElementById("socialmediaId").value;
        //const personId = document.getElementById("personId").value;
        const userEmail = document.getElementById("userEmail").value;

        if (!socialmediaId) {
            alert("Invalid Socialmedia ID.");
            return;
        }

        const confirmed = confirm("Are you sure you want to delete this socialmedia?");
        if (!confirmed) return;

        try {
            await SocialMediaService.delete(socialmediaId);
            alert("Socialmedia deleted.");
            window.location.href = `/dex/u/${userEmail}/p/${counter}/cont/list/soc`;
        } catch (error) {
            console.error("Delete failed:", error);
            alert("Unable to delete socialmedia.");
        }
    });
});

async function returnSocialmediaData() {
    const socialmediaId = document.getElementById("socialmediaId").value;
    const item = await SocialMediaService.getOne(socialmediaId);
    return item;
}

async function populateSocialmediaData(socialMedia) {
    try {
        
        //const socialmediaId = document.getElementById("socialmediaId").value;
        const _CategoryField = document.getElementById("_CategoryField");
        const _SocialHandle = document.getElementById("_SocialHandle");
        const _LogTimestamp = document.getElementById("_LogTimestamp");
        //const item = await returnSocialmediaData();

        // const personLc = item.localCounter;
        console.log("populateSocialmediaData(): item: ", socialMedia);
        
        DOM.removeChildren(_CategoryField);
        DOM.removeChildren(_SocialHandle);
        DOM.removeChildren(_LogTimestamp);
        
        DOM.setElementText("#_LogTimestamp",  socialMedia.logTimestamp);
        DOM.setElementText("#_CategoryField",    socialMedia.categoryField);
        DOM.setElementText("#_SocialHandle",    socialMedia.socialHandle);



        const form = document.getElementById('deleteSocialmediaForm');


        const innerDiv = document.createElement('div');
        innerDiv.innerHTML =
            `<button type="submit" class="btn btn-danger">Delete Socialmedia</button>
             <a class="btn btn-dark" href="/dex/u/${socialMedia.applicationUserName}/p/${socialMedia.localCounter}/cont/soc/${socialMedia.socialMediaId}">Cancel</a> | 
             <a class="btn btn-outline-dark" href="/dex/u/${socialMedia.applicationUserName}/p/${socialMedia.localCounter}/cont/list/soc">Back to List</a>`;
        console.log("div buttons to add", innerDiv);
        
        form.appendChild(innerDiv);
        
        
        const headContainer = document.getElementById("headContainer");
        let editHeading = document.getElementById("socialmediaHead");
        DOM.removeChildren(headContainer);
        
        //console.log("heading: ",editHeading);
        //DOM.removeChildren(editHeading);
        
        const h4 = document.createElement("h4");
        const textNode = document.createTextNode(`Delete SocialMedia #_${socialMedia.socialMediaId} ?`);
        h4.appendChild(textNode);


        //editHeading.textContent(`Delete SocialMedia #_${socialMedia.socialMediaId} ?`);
        editHeading.appendChild(h4);
        
        console.log("heading:  ", editHeading);


    } catch (error) {
        console.error("Failed to populate socialmedia Data.", error);

    }

}
