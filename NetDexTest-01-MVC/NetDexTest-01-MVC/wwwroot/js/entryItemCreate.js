"use strict";

import { EntryItemService } from "./entryItemHandler.js";
import { PersonRepository } from "./PersonRepository.js";
const personRepo = new PersonRepository("https://localhost:7134/api/people");

window.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("createEntryForm");

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        try {
            const result = await EntryItemService.create(form);
            alert("Entry created successfully.");
            const personId = form.PersonId.value;
            const userEmail = form.ApplicationUserEmail.value;
            
            try {
                const pResult = personRepo.readGlobally(userEmail, personId, "global")
                const pLocator = pResult.localCounter;
                console.log("pLocator result: ", pLocator);
                window.location.href = `/dex/u/${userEmail}/p/${pLocator}/rec/list/ie`;
            } catch (error) {
                console.error("ERROR: an error occurred while searching for a person using PersonId: ", error);
                var previewText = "";
                if(form.ShortTitle.value == null) { previewText = form.FlavorText.value; }
                else { previewText = form.ShortTitle.value; }
                alert(`NOTICE: The following Entry was created, but a LocalCounter or Nickname could not be found. Redirecting to PersonList instead of EntryList.\n\nEntry Preview:\n\t\"${previewText}\" `)
                window.location.href = `/dex/u/${userEmail}`;
            }
            
        } catch (err) {
            console.error("Create error:", err);
            alert("Failed to create entry.");
        }
    });
});
