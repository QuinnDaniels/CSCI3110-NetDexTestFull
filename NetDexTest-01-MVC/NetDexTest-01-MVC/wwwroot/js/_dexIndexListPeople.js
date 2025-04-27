"use strict";

import { DOM } from "./DOMCreator.js"; // import repository
import { UserRepository } from "./UserRepository.js"; // import repository
const userRepo = new UserRepository("https://localhost:7134/api/user"); // instantiate repository, set base address

//const f = new Date("2015-03-25T12:00:00-06:00");
//const d = f.getDate();
//const m = f.getMonth() + 1;
//const y = f.getFullYear();

//const date = new String(`${d}` + "-" + `${m}` + "-" + `${y}`);



const email = document.getElementById("email").value;
DOM.logElementToConsole(email);
const dexResponse = await userRepo.readDex(email);
console.log(dexResponse);



/*--------------------------------*/
async function standardFetchTest() {
    const response = await fetch(`https://localhost:7134/api/user/dex/${email}`);
    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    else {
        console.log(response);
        const data = await response.json();
        console.log(data);
        console.log(data.Roles);
    }
}
/*--------------------------------*/
async function populatePeopleList(userRepo, PersonDexListVM) {

    let h3DexId = document.getElementById("dexIdHead");              //h3 -> append(textNode) 
    let h3UserName = document.getElementById("userNameHead");        //h3 -> append(textNode)
    let h4PeopleCount = document.getElementById("peopleCountHead");  //h4 -> append(textNode)
    let ddGender = document.getElementById("genderField");           //dd -> append(textNode)
    let ddPronouns = document.getElementById("pronounsField");       //dd -> append(textNode)
    let ddBirthday = document.getElementById("birthdayField");       //dd -> append(textNode)
    let spanUserEmail = document.getElementById("userEmailSpan");  //span -> append(textNode)
    let spanFirst = document.getElementById("firstNameField");     //span -> append(textNode)
    let spanMiddle = document.getElementById("middleNameField");   //span -> append(textNode)
    let spanLast = document.getElementById("lastNameField");       //span -> append(textNode)





    let elemArray = [
        h3DexId,
        h3UserName,
        h4PeopleCount,
        ddGender,
        ddPronouns,
        ddBirthday,
        spanUserEmail,
        spanFirst,
        spanMiddle,
        spanLast
    ]

    let textArray = [
        DexHolderMiddleVM.DexId,
        DexHolderMiddleVM.ApplicationUserName,
        DexHolderMiddleVM.PeopleCount,
        DexHolderMiddleVM.Gender,
        DexHolderMiddleVM.Pronouns,
        DexHolderMiddleVM.DateOfBirth,
        DexHolderMiddleVM.ApplicationEmail,
        DexHolderMiddleVM.FirstName,
        DexHolderMiddleVM.MiddleName,
        DexHolderMiddleVM.LastName
    ]

    if (elemArray.length == textArray.length) {
        console.info("Beginning loop...");


        //console.log("people count: " , textArray[2]);
        if (elemArray[2] == "") { textArray[2] = 0; }
        //console.log("people count: ", textArray[2]);

        //console.log("HERE: application email: " , textArray[6]);


        for (let i = 0; i < elemArray.length; i++) {
            try {

                //console.log(`${textArray[i]} :`, elemArray[i])

                //console.info("Attempting to remove children...");
                try {
                    DOM.removeChildren(elemArray[i]);
                    console.log("Children Removed: ", elemArray[i])
                } catch (e) {
                    console.warn("ERROR: Could not remove element children! :", e)
                }


                //console.info("Setting element text directly...");
                DOM.setDirectElementTextWithPlaceholder(elemArray[i], textArray[i]);

                console.log("Element Text Set: ", elemArray[i]);
            } catch (e) {
                console.error(`An Error Occured within the For Loop!! - ${textArray[i]}`, e);
            }
        }
        console.info("loop complete!");

    }
    else {
        console.log("ERROR: element and text array must be the same length!!")
    }



    try {
        let tdRole = document.getElementById("userRolesCell");
        console.log(tdRole);
        try {
            DOM.removeChildren(tdRole);
        } catch (e) {
            console.log("ERROR: Could not remove children!!", tdRole, e)
        }
        const h5 = document.createElement('h5');
        h5.appendChild(document.createTextNode("Role(s):"));
        console.log(h5);
        tdRole.appendChild(h5);
        console.log(tdRole);
        const ulRole = DOM.createULfromArrayProperty(DexHolderMiddleVM.Roles, "RoleName")
        console.log(ulRole);
        tdRole.appendChild(ulRole);
        console.log(tdRole);
    } catch (e) {
        console.log("ERROR: something went wrong while making roles!!", e)

    }



}
