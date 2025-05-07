"use strict";

import { EntryItemService } from "./entryItemHandler.js";

window.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("createEntryForm");

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        try {
            const result = await EntryItemService.create(form);
            alert("Entry created successfully.");
            const personId = form.PersonId.value;
            const userEmail = form.ApplicationUserEmail.value;
            window.location.href = `/dex/u/${userEmail}/p/${personId}/rec/list/ie`;
        } catch (err) {
            console.error("Create error:", err);
            alert("Failed to create entry.");
        }
    });
});
