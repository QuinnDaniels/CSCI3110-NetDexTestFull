"use strict";

import { EntryItemService } from "./entryItemHandler.js";
import { DOM } from "./DOMCreator.js";

window.addEventListener("DOMContentLoaded", async () => {
    const form = document.getElementById("deleteEntryForm");

    const item = await returnEntryData();
    console.log("populateEntryData(): item: ", item);

    await populateEntryData(item);
    const headContainer = document.getElementById("headContainer");

    const entryHeading = document.getElementById("entryHead");
    DOM.removeChildren(headContainer);
    headContainer.appendChild(
        DOM.createImg("/images/ajax-loader.gif", "Loading image"));

    const localCounter = item.localCounter;

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const counter = localCounter;

        const entryId = document.getElementById("entryId").value;
        //const personId = document.getElementById("personId").value;
        const userEmail = document.getElementById("userEmail").value;

        if (!entryId) {
            alert("Invalid Entry ID.");
            return;
        }

        const confirmed = confirm("Are you sure you want to delete this entry?");
        if (!confirmed) return;

        try {
            await EntryItemService.delete(entryId);
            alert("Entry deleted.");
            window.location.href = `/dex/u/${userEmail}/p/${counter}/rec/list/ie`;
        } catch (error) {
            console.error("Delete failed:", error);
            alert("Unable to delete entry.");
        }
    });
});

async function returnEntryData() {
    const entryId = document.getElementById("entryId").value;
    const item = await EntryItemService.getOne(entryId);
    return item;
}

async function populateEntryData(entryItem) {
    try {
        
        //const entryId = document.getElementById("entryId").value;
        const _ShortTitle = document.getElementById("_ShortTitle");
        const _FlavorText = document.getElementById("_FlavorText");
        const _LogTimestamp = document.getElementById("_LogTimestamp");
        //const item = await returnEntryData();

        // const personLc = item.localCounter;
        console.log("populateEntryData(): item: ", entryItem);
        
        DOM.removeChildren(_ShortTitle);
        DOM.removeChildren(_FlavorText);
        DOM.removeChildren(_LogTimestamp);
        
        DOM.setElementText("#_LogTimestamp",  entryItem.logTimestamp);
        DOM.setElementText("#_ShortTitle",    entryItem.shortTitle);
        DOM.setElementText("#_FlavorText",    entryItem.flavorText);



        const form = document.getElementById('deleteEntryForm');


        const innerDiv = document.createElement('div');
        innerDiv.innerHTML =
            `<button type="submit" class="btn btn-danger">Delete Entry</button>
             <a class="btn btn-dark" href="/dex/u/${entryItem.applicationUserName}/p/${entryItem.localCounter}/rec/ie/${entryItem.entryItemId}">Cancel</a> | 
             <a class="btn btn-outline-dark" href="/dex/u/${entryItem.applicationUserName}/p/${entryItem.localCounter}/rec/list/ie">Back to List</a>`;
        console.log("div buttons to add", innerDiv);
        
        form.appendChild(innerDiv);
        
        
        const headContainer = document.getElementById("headContainer");
        let editHeading = document.getElementById("entryHead");
        DOM.removeChildren(headContainer);
        
        //console.log("heading: ",editHeading);
        //DOM.removeChildren(editHeading);
        
        const h4 = document.createElement("h4");
        const textNode = document.createTextNode(`Delete EntryItem #_${entryItem.entryItemId} ?`);
        h4.appendChild(textNode);


        //editHeading.textContent(`Delete EntryItem #_${entryItem.entryItemId} ?`);
        editHeading.appendChild(h4);
        
        console.log("heading:  ", editHeading);


    } catch (error) {
        console.error("Failed to populate entry Data.", error);

    }

}
