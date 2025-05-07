"use strict";

import { loadList } from "./entryItemHandler.js";
import { readPersonForChecks } from "./entryItemHandler.js";

window.addEventListener("DOMContentLoaded", () => {
    const userId = document.getElementById("userId").value;
    const personId = document.getElementById("personId").value;
    if (userId && personId) {
        // try {
            const personLinker = document.getElementById("emergencyLink");
            console.log("personLinker: ", personLinker);

            readPersonForChecks(userId, personId);
            loadList(userId, personId);
        // } catch (error) {
        //     console.error("ERROR: ", error);
        // }
    } else {
        console.error("Missing userId or personId");
    }
});
