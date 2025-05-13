"use strict";

import { DOM } from "./DOMCreator.js";


try {
    let socialmediaHeading = document.getElementById("socialmediaHeading");
    console.log("SocialMediaHeading: ", socialmediaHeading);
    DOM.removeChildren(socialmediaHeading);
    socialmediaHeading.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));

} catch (e) {
    console.log("heading error:", e);


} 

async function fetchSocialMedia(socialMediaId) {
    const url = `https://localhost:7134/api/socialmedia/transfer/one/${socialMediaId}`;
    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error("HTTP error when fetching SocialMedia");
        const item = await response.json();
        console.log("SocialMedia: ", item);
        const email = document.getElementById("email").value;
        const username = document.getElementById("username").value;
        
        const container = document.getElementById("socialMediaContainer");
        if (!container) return;
        

        const lastPerson = document.getElementById("lastPerson").value;
        const stamptime = new Date(item.logTimestamp);
        console.log("Timestamp: ", stamptime.toString());
        const stamp = stamptime.toString();
        console.log("stamp: ", stamp);
        
        const _miniView = document.getElementById("miniView");
        const _title = document.getElementById("title");
        const _flavor = document.getElementById("flavor");
        const _dateLogged = document.getElementById("dateLogged");
        
        const categoryfield = document.getElementById("dd_CategoryField");
        const socialhandle = document.getElementById("dd_SocialHandle");
        const logtimestamp = document.getElementById("dd_LogTimestamp");
        const socialmediaid = document.getElementById("dd_SocialMediaId").toString();
        const contactinfoid = document.getElementById("dd_ContactInfoId").toString();

        DOM.removeChildren(socialmediaid);
        DOM.removeChildren(categoryfield);
        DOM.removeChildren(socialhandle);
        DOM.removeChildren(logtimestamp);
        DOM.removeChildren(contactinfoid);
        
        DOM.setElementText("#dd_CategoryField", item.categoryField);
        DOM.setElementText("#dd_SocialHandle", item.socialHandle);
        DOM.setElementText("#dd_LogTimestamp", stamp);
        DOM.setElementText("#dd_SocialMediaId", item.socialMediaId.toString());
        
        console.log("LINE HERE----------------");
        
        DOM.setElementText("#dd_ContactInfoId", item.contactInfoId.toString());
        
        // DOM.setElementText("#dd_title", shortitle);
        // DOM.setElementText("#dd_flavor", item.socialHandle);
        // DOM.setElementValue("#dd_dateLogged", stamp);
        
        
        
        const linker = document.getElementById("socialmediaRedirectHolder");
        DOM.removeChildren(socialmediaRedirectHolder);
        linker.appendChild(DOM.socialMediaDetailsButtons(email, lastPerson, item.socialMediaId.toString())); //, contacts, contacts));
        
        let socialmediaHeading = document.getElementById("socialmediaHeading");
        DOM.removeChildren(socialmediaHeading);
        

        socialmediaHeading.appendChild(document.createTextNode(` SocialMedia: "${item.categoryField}"`));


    } catch (err) {
        console.error("Error loading SocialMedia:", err);
    }
}

window.onload = () =>  {
    const id = document.getElementById("socialMediaId").value.toString();
     fetchSocialMedia(id);
};



// async function DOMLoadFunction() {
//     console.log("DOM populateSocialmediaData reached");
//     const updateSocialmediaForm = document.getElementById("editSocialmediaForm");
    
//     const form = updateSocialmediaForm;
//     const contact =  document.getElementById("_contact").value;
//     const socialmedia =  document.getElementById("_socialmedia").value;
//     const CategoryField =  document.getElementById("_CategoryField").value;
//     const SocialHandle =  document.getElementById("_SocialHandle").value;
//     await populateSocialmediaData();
//     //console.log("contactsId:", contactsId);
    
    
//     form.addEventListener("submit", async (e) => {
//         e.preventDefault();
//         console.log("DOM submit event reached");
//         const form = e.target;
//         console.log("form: ", form);
//         const formData = new FormData(updateSocialmediaForm);
//         //const formData = new FormData(form);
//         for (const [key, value] of formData.entries()) {        // 3. Loop through and output the form data to the console
//             console.log(key, value);
//         }


//         try {
//             const result = await SocialmediaItemService.update(updateSocialmediaForm);
//             console.log("Result: ", result);
//             alert(`Socialmedia updated successfully! Redirecting...`);// \nResult: ${result}\n`);
//             //window.location.replace(`/dex/u/${email}/p/${numId}/rec/ie/${_socialmedia}`);
//             window.location.replace(`/dex/u/${email}/p/${numId}/cont/list/soc`);

//             //const personId = form.PersonId.value;
//             //const userEmail = form.ApplicationUserEmail.value;
//             //window.location.href = `/dex/u/${userEmail}/p/${numId}/rec/ie/${socialmediaId}`;
//             //window.location.replace(`/dex/u/${currentEmail}`);

//         } catch (err) {
//             console.error("Edit error:", err);
//             alert("Failed to update socialmedia.");
//         }
//     });
// }


