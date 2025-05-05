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

        DOM.setElementText("#title", item.shortTitle);
        DOM.setElementText("#flavor", item.flavorText);
        DOM.setElementText("#dateLogged", item.logTimestamp);

    } catch (err) {
        console.error("Error loading EntryItem:", err);
    }
}

window.onload = () => {
    const id = document.getElementById("entryItemId").value;
    fetchEntryItem(id);
};
