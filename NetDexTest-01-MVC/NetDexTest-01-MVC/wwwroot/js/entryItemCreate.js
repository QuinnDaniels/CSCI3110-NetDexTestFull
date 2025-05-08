"use strict";

import { EntryItemService } from "./entryItemHandler.js";
import { PersonRepository } from "./PersonRepository.js";
import { DOM } from "./DOMCreator.js";

const personRepo = new PersonRepository("https://localhost:7134/api/people");

const email  = document.getElementById("_LoggedInEmail");
console.info(email.value);
const person = document.getElementById("_LastPerson");

const info = document.getElementById("backupvalues");
console.log(`User: ${info._LoggedInEmail.value}
    \nLastPerson: ${info._LastPerson.value}
    \n
    \nDIRECT SELECTION:
    \nLastPerson: ${person.value}
    \nUser: ${email.value}
    \n`);
    
    
const mail = document.getElementById("email");
const laster = document.getElementById("lastperson");
const user = mail.value;
const personOut = laster.value;
console.log(mail);



window.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("createEntryForm");
    



    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        console.log("BUTTON CLICK: Attempting to create a person!");
        console.log("createEntryForm", form);

        try {
            const result = await EntryItemService.create(form);
            alert("Entry created successfully.");
            // const personId = form.PersonId.value;
            const personId =
                result.recordCollector.person.id;
            const entryId =
                result.id;


            //const userEmail = form.ApplicationUserEmail.value;

            console.log("personId: ", personId);
            const responder = await personRepo.readNickname(personId);
            console.log("responder: ", responder);
            const nickname = responder.nickname;
            console.log("nickname: ", nickname);

            try {
                //const pResult = personRepo.readGlobally(mail, personId, "global")
                //const pLocator = pResult.localCounter;
                const router = `/dex/u/${user}/p/${personId}/rec/ie/${entryId}`;
                console.log("REDIRECT COMMAND REACHED - success", router);
                window.location.replace(router);
                //indow.location.href = `/dex/u/${userEmail}/p/${pLocator}/rec/list/ie`;
            } catch (error) {
                console.error("ERROR: an error occurred while searching for a person using PersonId: ", error);
                var previewText = "";
                if(form.ShortTitle.value == null) { previewText = form.FlavorText.value; }
                else { previewText = form.ShortTitle.value; }
                alert(`NOTICE: The following Entry was created, but a LocalCounter or Nickname could not be found. Redirecting to PersonList instead of EntryList.\n\nEntry Preview:\n\t\"${previewText}\" `)
                
                //const router = `/dex/u/${user}`;
                const router = `/dex/u/${user}/p/${personOut}/create/rec/ie`;
                console.log("REDIRECT COMMAND REACHED - catch 1", router);
                window.location.replace(router);
//                window.location.href = `/dex/u/${userEmail}`;
            }
            // TODO - fix the redirect!!!
            
            
            
            
            //const router = `/dex/u/${user}/p/${personOut}/create/rec/ie`;


            const router = `/dex/u/${user}/p/${personId}/rec/list/ie`;

            console.log("REDIRECT COMMAND REACHED", router);
            //            window.location.href = router;
            alert(``);
            window.location.replace(router);
            
        } catch (err) {
            console.error("Create error:", err);
            alert("Failed to create entry.");


        }
    });
});
