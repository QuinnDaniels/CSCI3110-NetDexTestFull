"use strict";

import { DOM } from "./DOMCreator.js";
// import { PersonRepository } from "./PersonRepository.js";
// const personRepo = new PersonRepository("https://localhost:7134/api/people");

const apiBase = "https://localhost:7134/api/socialmedia";

export const SocialMediaService = {
    async getOne(id) {
        const res = await fetch(`${apiBase}/transfer/one/${id}`);
        if (!res.ok) throw new Error("Failed to fetch socialmedia item.");
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
        if (res.ok != true){ throw new Error("Failed to create socialmedia."); }
        return await res.json();
    },

    async update(form) {
        console.log("update(form): ", form);
        const formData = new FormData(form);

        let stringer = ""
        for (const [key, value] of formData.entries()) {        // 3. Loop through and output the form data to the console
            console.log(key, value);
            stringer += `\n\t${key} = ${value}`
        }
        formAlert(stringer);
    
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
            throw new Error("Failed to update socialmedia.");
        } 
    },

    async delete(id) {
        const res = await fetch(`${apiBase}/delete/${id}`, { method: "DELETE" });
        if (!res.ok) throw new Error("Failed to delete socialmedia.");
    }
};

// Example usage in page
export async function loadDetails(id) {
    try {
        const socialmedia = await SocialMediaService.getOne(id);
        DOM.setElementText("#socialmediaTitle", socialmedia.categoryField);
        DOM.setElementText("#socialmediaFlavor", socialmedia.socialHandle);
        DOM.setElementText("#socialmediaLogged", new Date(socialmedia.logTimestamp).toLocaleString());
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
                console.info("readPersonForChecks(): ", addresser);
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
    

    const addresser = `https://localhost:7134/api/socialmedia/transfer/person/${input}/${personId}`;
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
        const list = await SocialMediaService.getByPerson(input, personId);
        //const locallist = await SocialMediaService.ReadPersonForChecks(input, personId);
        const tbody = document.getElementById("socialmediaTableBody");
        DOM.removeChildren(tbody);
        
        list.forEach(socialmedia => {
            const row = document.createElement("tr");
            row.appendChild(DOM.createTextTD(socialmedia.categoryField));
            row.appendChild(DOM.createTextTD(socialmedia.socialHandle));
            row.appendChild(DOM.createTextTD(new Date(socialmedia.logTimestamp).toLocaleString()));
            
            // ~T~O~D~O~ - [[REPLICATE]] THIS TO OTHER HANDLERS
            // HACK - do a nested for loop or if statementto find where person id matches, and get the local counter.
            if (socialmedia.personId == locallist.id){
                temp = locallist.localCounter;
            } // fix this later to include new LocalCounter field on socialMedias

            row.appendChild(DOM.socialMediaListButtons(socialmedia.applicationUserEmail, socialmedia.localCounter, socialmedia.socialMediaId))
            //row.appendChild(DOM.createButtonLink("View", `/dex/u/${socialmedia.applicationUserEmail}/p/${socialmedia.personId}/cont/soc/${socialmedia.socialMediaId}`, 'info'));
            
            
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
