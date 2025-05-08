"use strict";

import { SocialMediaService } from "./socialMediaHandler.js";
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
    const form = document.getElementById("createSocialmediaForm");
    



    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        console.log("BUTTON CLICK: Attempting to create a person!");
        console.log("createSocialmediaForm", form);

        try {
            const result = await SocialMediaService.create(form);
            alert("Socialmedia created successfully.");
            // const personId = form.PersonId.value;
            const personId =
                result.contactInfo.person.id;
            const socialmediaId =
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
                const router = `/dex/u/${user}/p/${personId}/cont/soc/${socialmediaId}`;
                console.log("REDIRECT COMMAND REACHED - success", router);
                window.location.replace(router);
                //indow.location.href = `/dex/u/${userEmail}/p/${pLocator}/cont/list/soc`;
            } catch (error) {
                console.error("ERROR: an error occurred while searching for a person using PersonId: ", error);
                var previewText = "";
                if(form.CategoryField.value == null) { previewText = form.SocialHandle.value; }
                else { previewText = form.CategoryField.value; }
                alert(`NOTICE: The following Socialmedia was created, but a LocalCounter or Nickname could not be found. Redirecting to PersonList instead of SocialmediaList.\n\nSocialmedia Preview:\n\t\"${previewText}\" `)
                
                //const router = `/dex/u/${user}`;
                const router = `/dex/u/${user}/p/${personOut}/create/cont/soc`;
                console.log("REDIRECT COMMAND REACHED - catch 1", router);
                window.location.replace(router);
//                window.location.href = `/dex/u/${userEmail}`;
            }

            //const router = `/dex/u/${user}/p/${personOut}/create/cont/soc`;


            const router = `/dex/u/${user}/p/${personId}/cont/list/soc`;

            console.log("REDIRECT COMMAND REACHED", router);
            //            window.location.href = router;
            alert(``);
            window.location.replace(router);
            
        } catch (err) {
            console.error("Create error:", err);
            alert("Failed to create socialmedia.");


        }
    });
});
