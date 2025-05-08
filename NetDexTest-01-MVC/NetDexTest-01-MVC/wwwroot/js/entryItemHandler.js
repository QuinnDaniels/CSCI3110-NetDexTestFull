"use strict";

import { DOM } from "./DOMCreator.js";
// import { PersonRepository } from "./PersonRepository.js";
// const personRepo = new PersonRepository("https://localhost:7134/api/people");

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
        console.log(res);
        console.log("response: ", res.status, res.statusText);
        console.log("response json: ", res.json);
        console.log("response ok: ", res.ok);
        if (res.ok != true){ throw new Error("Failed to create entry."); }
        return await res.json();
    },
    
    async update(form) {
        console.log("update(form): ", form);
        const formData = new FormData(form);
        const res = await fetch(`${apiBase}/put`, {
            method: "PUT",
            body: formData
        });
        console.log("update response: ", res);
        console.log("update response: ", res.status, res.statusText);
        console.log("update response json: ", res.json);
        console.log("update response ok: ", res.ok);
        if (!res.ok){
            alert("failed to update");
            throw new Error("Failed to update entry.");
        } 
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





export async function readPersonForChecks(input, criteria) {
        const addresser = `https://localhost:7134/api/people/retrieveRequestpath/${input}/${criteria}`;
        //return await response.json();
        console.info("fn ReadPersonForChecks() - requested address: ",addresser);
        const response = await fetch(addresser, {
            method: "GET",
            //body: formData
        });
        try{
            console.info("readPersonForChecks(): ", "response: ", response, response.body);
            if (!response.ok) {
                console.info("readPersonForChecks(): ", "status code",response.status);
                console.info("readPersonForChecks(): ", "status message",response.statusText);
                console.info("readPersonForChecks(): ", address);
                throw new Error("There was an HTTP error creating the person data.");
            }
            const result = await response.json();
            console.info("readPersonForChecks(): ", "Success:", result);
            return result;
        }
        catch(error) {
            console.error("readPersonForChecks(): ", "Error:", error);
        }
    }


export async function loadList(input, personId) {
    
    var temp = 0;
    

    const addresser = `https://localhost:7134/api/people/retrieveRequestpath/${input}/${personId}`;
        //return await response.json();
        
    // const response = await fetch(personId, {
    //     method: "GET",
    //     //body: formData
    // });
    const response = await fetch(addresser, {
        method: "GET",
        //body: formData
    });
        // try{
    console.log("loadList():", "response: ", response, response.body);
    if (!response.ok) {
        console.log("loadList():", "status code",response.status);
        console.log("loadList():", "status message",response.statusText);
        console.log("loadList():", address);
        throw new Error("There was an HTTP error creating the person data.");
    }
    const result = await response.json();
    console.log("loadList():", "Success:", result);
    const locallist = result;
        // }
        // catch(error) {
        //     console.error("Error:", error);
        // }
    

    
    
    
    try {
        const list = await EntryItemService.getByPerson(input, personId);
        //const locallist = await EntryItemService.ReadPersonForChecks(input, personId);
        const tbody = document.getElementById("entryTableBody");
        DOM.removeChildren(tbody);
        
        list.forEach(entry => {
            const row = document.createElement("tr");
            row.appendChild(DOM.createTextTD(entry.shortTitle));
            row.appendChild(DOM.createTextTD(entry.flavorText));
            row.appendChild(DOM.createTextTD(new Date(entry.logTimestamp).toLocaleString()));
            
            // ~T~O~D~O~ - [[REPLICATE]] THIS TO OTHER HANDLERS
            // HACK - do a nested for loop or if statementto find where person id matches, and get the local counter.
            if (entry.personId == locallist.id){
                temp = locallist.localCounter;
            } // fix this later to include new LocalCounter field on entryItems

            row.appendChild(DOM.entryListButtons(entry.applicationUserEmail, temp, entry.entryItemId))
            //row.appendChild(DOM.createButtonLink("View", `/dex/u/${entry.applicationUserEmail}/p/${entry.personId}/rec/ie/${entry.entryItemId}`, 'info'));
            
            
            tbody.appendChild(row);
        });
    } catch (err) {
        console.error("loadList():","List load error:", err);
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
