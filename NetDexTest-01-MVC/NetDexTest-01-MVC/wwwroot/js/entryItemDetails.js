"use strict";

import { DOM } from "./DOMCreator.js";


try {
    let entryHeading = document.getElementById("entryHeading");
    DOM.removeChildren(entryHeading);
    entryHeading.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));

} catch (e) {
    console.log("heading error:", e);


} 

async function fetchEntryItem(entryItemId) {
    const url = `https://localhost:7134/api/entry/transfer/one/${entryItemId}`;
    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error("HTTP error when fetching EntryItem");
        const item = await response.json();
        console.log("EntryItem: ", item);
        const email = document.getElementById("email").value;
        const username = document.getElementById("username").value;
        
        const container = document.getElementById("entryItemContainer");
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

        const shorttitle = document.getElementById("dd_ShortTitle");
        const flavortext = document.getElementById("dd_FlavorText");
        const logtimestamp = document.getElementById("dd_LogTimestamp");
        const entryitemid = document.getElementById("dd_EntryItemId");
        const recordcollectorid = document.getElementById("dd_RecordCollectorId");

        DOM.removeChildren(entryitemid);
        DOM.removeChildren(shorttitle);
        DOM.removeChildren(flavortext);
        DOM.removeChildren(logtimestamp);
        DOM.removeChildren(recordcollectorid);

        DOM.setElementText("#dd_ShortTitle", item.shortTitle);
        DOM.setElementText("#dd_FlavorText", item.flavorText);
        DOM.setElementText("#dd_LogTimestamp", stamp);
        DOM.setElementText("#dd_EntryItemId", item.entryItemId);
        DOM.setElementText("#dd_RecordCollectorId", item.recordCollectorId);

        // DOM.setElementText("#dd_title", shortitle);
        // DOM.setElementText("#dd_flavor", item.flavorText);
        // DOM.setElementValue("#dd_dateLogged", stamp);



        const linker = document.getElementById("entryRedirectHolder");
        DOM.removeChildren(entryRedirectHolder);
        linker.appendChild(DOM.entryItemDetailsButtons(email, lastPerson, item.entryItemId)); //, records, contacts));

        let entryHeading = document.getElementById("entryHeading");
        DOM.removeChildren(entryHeading);
        

        entryHeading.appendChild(document.createTextNode(` EntryItem: "${item.shortTitle}"`));


    } catch (err) {
        console.error("Error loading EntryItem:", err);
    }
}

window.onload = () =>  {
    const id = document.getElementById("entryItemId").value;
     fetchEntryItem(id);
};
