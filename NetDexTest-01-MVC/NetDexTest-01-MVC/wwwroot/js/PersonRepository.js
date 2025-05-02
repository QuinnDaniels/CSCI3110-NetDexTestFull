"use strict";


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
    async read(input, criteria) {
        const address = `${this.#baseAddress}/retrieveRequestpath/${input}/${criteria}`;
        //return await response.json();
        
        const response = await fetch(address, {
            method: "GET",
            //body: formData
        });
        //const data = { key1: 'value1', key2: 'value2' };
        //const data = { 'Input': input, 'Criteria': criteria };


        //const response = await makeHttpCallAsync('GET', address, data)
        //    .then(result => console.log('Success:', result))
        //    .catch(error => console.error('Failed:', error));



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
        }
        catch(error) {
            console.error("Error:", error);
        }
    



    async makeHttpCallAsync(method, url, data) {
        try {
            const response = await fetch(url, {
                method: method,
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error making HTTP call:', error);
            throw error;
        }
    }

// Example usage:
//const url = 'https://api.example.com/data';
//const data = { key1: 'value1', key2: 'value2' };

//makeHttpCallAsync(url, data)
//    .then(result => console.log('Success:', result))
//    .catch(error => console.error('Failed:', error));






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



    async update(formData, personId) {
        const address = `${this.#baseAddress}/forms/update/${personId}`; // /api/people
        console.log("Address:", address);
        const response = await fetch(address, {
            method: "put",
            body: formData
        });
        if (!response.ok) {
            throw new Error("There was an HTTP error updating the pet data.");
        }
        return await response.text();
    }

    async deletePerson(input, criteria) {
        const address = `${this.#baseAddress}/delete/${input}/${criteria}`;
        console.log("Address:", address);
        const response = await fetch(address, {
            method: "delete"
        });
        if (!response.ok) {
            throw new Error("There was an HTTP error deleting the person data.");
        }
        return await response.text();
    }



    }


