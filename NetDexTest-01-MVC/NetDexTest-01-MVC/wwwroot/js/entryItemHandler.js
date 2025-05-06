"use strict";

import { DOM } from "./DOMCreator.js";

const apiBase = "https://localhost:7134/api/entry";

export const EntryItemService = {
    async getOne(id) {
        const res = await fetch(`${apiBase}/transfer/one/${id}`);
        if (!res.ok) throw new Error("Failed to fetch entry item.");
        return await res.json();
    },

    async getByPerson(input, personId) {
        const res = await fetch(`${apiBase}/transfer/person/${input}/${personId}`);
        if (!res.ok) throw new Error("Failed to fetch entries for person.");
        return await res.json();
    },

    async getAll() {
        const res = await fetch(`${apiBase}/transfer/all`);
        if (!res.ok) throw new Error("Failed to fetch entries.");
        return await res.json();
    },

    async create(form) {
        const formData = new FormData(form);
        const res = await fetch(`${apiBase}/create`, {
            method: "POST",
            body: formData
        });
        if (!res.ok) throw new Error("Failed to create entry.");
        return await res.json();
    },

    async update(form) {
        const formData = new FormData(form);
        const res = await fetch(`${apiBase}/put`, {
            method: "PUT",
            body: formData
        });
        if (!res.ok) throw new Error("Failed to update entry.");
    },

    async delete(id) {
        const res = await fetch(`${apiBase}/delete/${id}`, { method: "DELETE" });
        if (!res.ok) throw new Error("Failed to delete entry.");
    }
};

// Example usage in page
export async function loadDetails(id) {
    try {
        const entry = await EntryItemService.getOne(id);
        DOM.setElementText("#entryTitle", entry.shortTitle);
        DOM.setElementText("#entryFlavor", entry.flavorText);
        DOM.setElementText("#entryLogged", new Date(entry.logTimestamp).toLocaleString());
    } catch (err) {
        console.error("Detail load error:", err);
    }
}

export async function loadList(input, personId) {
    try {
        const list = await EntryItemService.getByPerson(input, personId);
        const tbody = document.getElementById("entryTableBody");
        DOM.removeChildren(tbody);
        list.forEach(entry => {
            const row = document.createElement("tr");
            row.appendChild(DOM.createTextTD(entry.shortTitle));
            row.appendChild(DOM.createTextTD(entry.flavorText));
            row.appendChild(DOM.createTextTD(new Date(entry.logTimestamp).toLocaleString()));
            
            row.appendChild(DOM.entryListButtons(entry.applicationUserEmail, entry.personId, entry.entryItemId))
            //row.appendChild(DOM.createButtonLink("View", `/dex/u/${entry.applicationUserEmail}/p/${entry.personId}/rec/ie/${entry.entryItemId}`, 'info'));
            
            
            tbody.appendChild(row);
        });


        
    } catch (err) {
        console.error("List load error:", err);
    }
}

// Optional: form handling
export function hookFormSubmit(formId, handler) {
    const form = document.getElementById(formId);
    if (form) {
        form.addEventListener("submit", async (e) => {
            e.preventDefault();
            try {
                await handler(form);
                alert("Success!");
                window.location.reload();
            } catch (err) {
                alert(err.message);
            }
        });
    }
}
