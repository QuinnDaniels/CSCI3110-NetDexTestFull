"use strict";

import { DOM } from "./DOMCreator.js";

async function fetchEntryItem(entryItemId) {
    const url = `https://localhost:7134/api/entry/transfer/one/${entryItemId}`;
    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error("HTTP error when fetching EntryItem");
        const item = await response.json();
        console.log("EntryItem: ", item);

        const container = document.getElementById("entryItemContainer");
        if (!container) return;


         

        const shorttitle = document.getElementById("_ShortTitle");
        const flavortext = document.getElementById("_FlavorText");
        const logtimestamp = document.getElementById("_LogTimestamp");
        const entryitemid = document.getElementById("_EntryItemId");
        const recordcollectorid = document.getElementById("_RecordCollectorId");

        DOM.removeChildren(entryitemid);
        DOM.removeChildren(shorttitle);
        DOM.removeChildren(flavortext);
        DOM.removeChildren(logtimestamp);
        DOM.removeChildren(recordcollectorid);

        DOM.setElementText("#_ShortTitle", item.shortTitle);
        DOM.setElementText("#_FlavorText", item.flavorText);
        DOM.setElementValue("#_LogTimestamp", item.logTimestamp);
        DOM.setElementValue("#_EntryItemId", item.entryItemId);
        DOM.setElementValue("#_RecordCollectorId", item.recordCollectorId);

        DOM.removeChildren(entryRedirectHolder);
        const linker = document.getElementById("entryRedirectHolder");
        linker.appendChild(DOM.entryItemDetailsButtons(email, lastperson, item.entryItemId)); //, records, contacts));



    } catch (err) {
        console.error("Error loading EntryItem:", err);
    }
}

window.onload = () => {
    const id = document.getElementById("entryItemId").value;
    fetchEntryItem(id);
};
