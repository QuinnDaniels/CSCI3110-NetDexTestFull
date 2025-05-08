"use strict";

import { DOM } from "./DOMCreator.js";


try {
    let socialmediaHeading = document.getElementById("socialmediaHeading");
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
        DOM.setElementText("#dd_ContactInfoId", item.contactInfoId.toString());

        // DOM.setElementText("#dd_title", shortitle);
        // DOM.setElementText("#dd_flavor", item.socialHandle);
        // DOM.setElementValue("#dd_dateLogged", stamp);



        const linker = document.getElementById("socialmediaRedirectHolder");
        DOM.removeChildren(socialmediaRedirectHolder);
        linker.appendChild(DOM.socialMediaDetailsButtons(email, lastPerson, item.socialMediaId.toString())); //, records, contacts));

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
