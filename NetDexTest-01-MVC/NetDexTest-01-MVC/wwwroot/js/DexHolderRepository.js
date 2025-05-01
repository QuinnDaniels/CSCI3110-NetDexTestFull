"use strict";
export class DexHolderRepository { //create a class and export it (for use in UserIndex)
    #baseAddress;
    constructor(baseAddress) {
        this.#baseAddress = baseAddress;    // constructor initializes private member, #baseAddress, to the incoming argument
    }


    /*--------how & where to set the address-----*/
    // in userListAll.js (the js for the page):
    //
    // import { UserRepository } from "./UserRepository.js"; // import repository
    //
    //const userRepo = new UserRepository("https://localhost:7134/api/user");
    //      // instantiate repository,^^                    ^^^
    //                              //  set base address   __|
    //
    /*-------------------------------------------*/





    /*---------------------------*/
    // ReadAll
    //Given: Nothing
    //Returns: Collection of objects
    //1. Send the GET request
    //2. Await response
    //3. Check for errors
    //4. Return the collection
    /*--------------------------*/
    /**/
    async readAll() {
        const address = `${this.#baseAddress}/admin/all`; //`https://localhost:7134/api/user/admin/all`;
        const response = await fetch(address);
        if (!response.ok) {
            throw new Error("There was an HTTP error getting the boardgame data.");
        }
        return await response.json();
    }


    /*---------------------------*/
    //    Read
    //    Given: object id
    //    Returns: the object
    //    1.	Send the GET request
    //    2.	Await response
    //    3.	Check for errors
    //    4.	Return the object
    /*---------------------------*/
    /**/
    async read(input) {
        const address = `${this.#baseAddress}/dex/${input}`;
        const response = await fetch(address);
        if (!response.ok) {
            throw new Error("There was an HTTP error getting the dex data.");
        }
        return await response.json();
    }


}




export class PersonRepository { //create a class and export it (for use in UserIndex)
    #baseAddress;
    constructor(baseAddress) {
        this.#baseAddress = baseAddress;    // constructor initializes private member, #baseAddress, to the incoming argument
    }


    /*--------how & where to set the address-----*/
    // in userListAll.js (the js for the page):
    //
    // import { UserRepository } from "./UserRepository.js"; // import repository
    //
    //const userRepo = new UserRepository("https://localhost:7134/api/user");
    //      // instantiate repository,^^                    ^^^
    //                              //  set base address   __|
    //
    /*-------------------------------------------*/





    /*---------------------------*/
    // ReadAll
    //Given: Nothing
    //Returns: Collection of objects
    //1. Send the GET request
    //2. Await response
    //3. Check for errors
    //4. Return the collection
    /*--------------------------*/
    /**/
    async readAll() {
        const address = `${this.#baseAddress}/admin/all`; //`https://localhost:7134/api/user/admin/all`;
        const response = await fetch(address);
        if (!response.ok) {
            throw new Error("There was an HTTP error getting the boardgame data.");
        }
        return await response.json();
    }


    /*---------------------------*/
    //    Read
    //    Given: object id
    //    Returns: the object
    //    1.	Send the GET request
    //    2.	Await response
    //    3.	Check for errors
    //    4.	Return the object
    /*---------------------------*/
    /**/
    async read(input) {
        const address = `${this.#baseAddress}/dex/${input}`;
        const response = await fetch(address);
        if (!response.ok) {
            throw new Error("There was an HTTP error getting the dex data.");
        }
        return await response.json();
    }


    async create(formData) {
        const address = `${this.#baseAddress}/forms/create`;
        for (const [key, value] of formData.entries()) {        // 3. Loop through and output the form data to the console
            console.log(key, value);
        }

        const response = await fetch(address, {
            method: "POST",
            body: formData
        });
        console.log("response: ", response, response.body);
        if (!response.ok) {
            console.log(response.status);
            console.log(response.statusText);
            console.log(address);
            throw new Error("There was an HTTP error creating the person data.");
        }
        const result = await response.json();
        console.log("Success:", result);
        return result;
        } catch(error) {
            console.error("Error:", error);
        }
    }











