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
        DOM.setElementValue("#dd_LogTimestamp", item.logTimestamp);
        DOM.setElementValue("#dd_EntryItemId", item.entryItemId);
        DOM.setElementValue("#dd_RecordCollectorId", item.recordCollectorId);

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
