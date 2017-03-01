# University Registrar Management

#### By _**Alexandra Holcombe & Nicole Sanders**_

## Description

*  As a registrar, I want to enter a student, so I can keep track of all students enrolled at this University. I should be able to provide a name and date of enrollment.
*  As a registrar, I want to enter a course, so I can keep track of all of the courses the University offers. I should be able to provide a course name and a course number (ex. HIST100).
*  As a registrar, I want to be able to assign students to a course, so that teachers know which students are in their course. A course can have many students and a student can take many courses at the same time.

***

## Setup/Installation Requirements

#### Create Databases
* In `SQLCMD`:  
        `> CREATE DATABASE hair_salon`  
        `> GO`  
        `> USE hair_salon`  
        `> GO`  
        `> CREATE TABLE books (id INT IDENTITY(1,1), name VARCHAR(255));`  
        `> GO`  
        `> CREATE TABLE clients (id INT IDENTITY(1,1), name VARCHAR(255), book_id INT);`  
        `> GO`  

* Requires DNU, DNX, MSSQL, and Mono
* Clone to local machine
* Use command "dnu restore" in command prompt/shell
* Use command "dnx kestrel" to start server
* Navigate to http://localhost:5004 in web browser of choice

***

## Specifications

### Book Class
================  

**The DeleteAll method for the Book class will delete all rows from the books table.**
* Example Input: none
* Example Output: nothing

**The GetAll method for the Book class will return an empty list if there are no entries in the Book table.**
* Example Input: N/A, automatically loads on home page
* Example Output: empty list

**The Equals method for the Book class will return true if the Book in local memory matches the Book pulled from the database.**
* Example Input:  
        > Local: "War and Peace", id is 10  
        > Database: "War and Peace", id is 10  
* Example Output: `true`

**The Save method for the Book class will save new Books to the database.**
* Example Input:  
\> New book: "War and Peace"
* Example Output: no return value

**The Save method for the Book class will assign an id to each new instance of the Book class.**
* Example Input:  
\> New book: "War and Peace", `local id: 0`  
* Example Output:  
\> "War and Peace", `database-assigned id`  

**The GetAll method for the Book class will return all student entries in the database in the form of a list.**
* Example Input:  
        > "Of Mice and Men", id is 10  
        > "East of Eden", id is 11  
* Example Output: `{Of Mice and Men object}, {East of Eden object}`

**The Find method for the Book class will return the Book as defined in the database.**
* Example Input: "War and Peace"
* Example Output: "War and Peace", `database-assigned id`

**The Delete method for the Book class will Delete the book's row in the books table.**
* Example Input: *delete clicky* War and Peace
* Example Output: nothing

**The SearchByTitle method for the Book class will return the book object with a matching title**

### Author class
================

**The DeleteAll method for the Author class will delete all rows from the authors table.**
* Example Input: none
* Example Output: nothing

**The GetAll method for the Author class will return an empty list if there are no entries in the Author table.**
* Example Input: N/A, automatically loads on home page
* Example Output: empty list

**The Equals method for the Author class will return true if the Author in local memory matches the Author pulled from the database.**
* Example Input:  
        > Local: "H. G. Wells", id is 10  
        > Database: "H. G. Wells", id is 10  
* Example Output: `true`

**The Save method for the Author class will save new Authors to the database.**
* Example Input:  
\> New author: "H. G. Wells"
* Example Output: no return value

**The Save method for the Author class will assign an id to each new instance of the Author class.**
* Example Input:  
\> New author: "H. G. Wells"
* Example Output:  
\> "H. G. Wells"

**The GetAll method for the Author class will return all author entries in the database in the form of a list.**
* Example Input:  
        > "H. G. Wells"
        > "John Steinbeck"
* Example Output: `{H. G. Wells object}, {John Steinbeck object}`

**The Find method for the Author class will return the Author as defined in the database.**
* Example Input: "H. G. Wells"
* Example Output: "H. G. Wells", `database-assigned id`

**The Delete method for the Author class will Delete the book's row in the authors table.**
* Example Input: *delete clicky* "H. G. Wells"
* Example Output: nothing

**The Update method for the Book class will update a book's title or authors**


### Book && Author Classes
=========================  

**The AddAuthor method for the Book class will add a row to books_authors.**

**The AddBook method for the Author class will add a row to books_authors.**
* Example Input: Author: "H. G. Wells" Book: "War of the Worlds"
* Example Output: n/a

**The GetBooks method will return a list of books by the specified author.**
* Example Input: "H. G. Wells"
* Example Output: "War of the Worlds", "The Time Machine"

**The GetAuthors method will return all of a book's authors.**
* Example Input: "War of the Worlds"
* Example Output: "H. G. Wells"


### Copy class
================

**The DeleteAll method for the Copy class will delete all rows from the copies table.**
* Example Input: none
* Example Output: nothing

**The GetAll method for the Copy class will return an empty list if there are no entries in the Copy table.**
* Example Input: N/A, automatically loads on home page
* Example Output: empty list

**The Equals method for the Copy class will return true if the Copy in local memory matches the Copy pulled from the database.**
* Example Input:  
        > Local: "H. G. Wells", id is 10  
        > Database: "H. G. Wells", id is 10  
* Example Output: `true`

**The Save method for the Copy class will save new Copys to the database.**
* Example Input:  
\> New copy: "H. G. Wells"
* Example Output: no return value

**The Save method for the Copy class will assign an id to each new instance of the Copy class.**
* Example Input:  
\> New copy: "H. G. Wells"
* Example Output:  
\> "H. G. Wells"

**The GetAll method for the Copy class will return all copy entries in the database in the form of a list.**
* Example Input:  
        > "H. G. Wells"
        > "John Steinbeck"
* Example Output: `{H. G. Wells object}, {John Steinbeck object}`

**The Find method for the Copy class will return the Copy as defined in the database.**
* Example Input: "H. G. Wells"
* Example Output: "H. G. Wells", `database-assigned id`

**The Delete method for the Copy class will Delete the book's row in the copys table.**
* Example Input: *delete clicky* "H. G. Wells"
* Example Output: nothing


### Book && Copy Classes
=========================  

**The AddCopy method for the Book class will add a row to copies.**
* Example Input: Book: "War of the Worlds" Copy: `copy id`
* Example Output: n/a

**The CountCopies method for the Book class will return the number of rows in the copies class with a matching book_id**
* Example Input: "War of the Worlds"
* Example Output: 4 copies


### Checkout class
================

**The DeleteAll method for the Checkout class will delete all rows from the checkouts table.**
* Example Input: none
* Example Output: nothing

**The GetAll method for the Checkout class will return an empty list if there are no entries in the Checkout table.**
* Example Input: N/A, automatically loads on home page
* Example Output: empty list

**The Equals method for the Checkout class will return true if the Checkout in local memory matches the Checkout pulled from the database.**
* Example Input:  
        > Local: "Due 2017, 03, 15", id is 10  
        > Database: "Due 2017, 03, 15", id is 10  
* Example Output: `true`

**The Save method for the Checkout class will save new Checkouts to the database.**
* Example Input:  
\> New checkout: "Due 2017, 03, 15", `patrons_id`, `copies_id`
* Example Output: no return value

**The Save method for the Checkout class will assign an id to each new instance of the Checkout class.**
* Example Input:  
\> New checkout: "Due 2017, 03, 15", `patrons_id`, `copies_id`
* Example Output:  
\> "H. G. Wells"

**The GetAll method for the Checkout class will return all checkout entries in the database in the form of a list.**
* Example Input:  
        > "Due 2017, 03, 15"
        > "Due 2017, 03, 16"
* Example Output: `{checkout object}, {checkout object}`

**The Find method for the Checkout class will return the Checkout as defined in the database.**
* Example Input: "Due 2017, 03, 15"
* Example Output: "Due 2017, 03, 15", `database-assigned id`

**The Delete method for the Checkout class will Delete the checkout's row in the checkouts table.**
* Example Input: *delete clicky* "Due 2017, 03, 15"
* Example Output: nothing

**The Update method for the Checkout class will change the due date, patron_id, or copy_id in the database.**
* Example Input: "Due 2017, 03, 15", `patron_id`, `book_id`, checkout id is `14`
* Example Input: "Due 2017, 04, 15", new `patron_id`, new `book_id`, checkout id is `14`

### Checkout && Book && Copy Classes
=========================  

**The AvailableCopies method for the Book class will return the number of copies with the same books_id that are not also in the checkouts table.**
* Example Input:  Book: War of the Worlds Checked out copies: 4
* Example Output: Available Copies: 1

### Patron class
================

**The DeleteAll method for the Patron class will delete all rows from the patrons table.**

**The GetAll method for the Patron class will return an empty list if there are no entries in the Patron table.**

**The Equals method for the Patron class will return true if the Patron in local memory matches the Patron pulled from the database.**

**The Save method for the Patron class will save new Patrons to the database.**

**The Save method for the Patron class will assign an id to each new instance of the Patron class.**

**The GetAll method for the Patron class will return all copy entries in the database in the form of a list.**

**The Find method for the Patron class will return the Patron as defined in the database.**

**The Delete method for the Patron class will Delete the book's row in the patrons table.**

### User Interface
===================  

**The registrar can add a new Course using the "Add Course" form.**
* Example Input: New Course: "Jazz Hands"
* Example Output: All courses: "Jazz Hands", "Slow-Dancing", "Coloring"

**The registrar can add a new Student using the "Add New Student" form.**
* Example Input: New Student: "Jennifer", 02/28/2017
* Example Output: All students: "Allison, Kacey, Jennifer"

**The registrar can add a student to a course using the "Add to Class" form.**
* Example Input: "Marc", add to "Chorus"
* Example Output: All students in Chorus: "Marc", "Christ"

**The registrar can see a list of all students in a course by clicking on the course name.**
* Example Input: "Remedial Physics"
* Example Output: "Remedial Physics" Students: "Marc"

**The registrar can see a list of all courses taken by a student by clicking on the student.**
* Example Input: "Marc"
* Example Output: "Jazz Hands for Jesus", "Sprinting", "Power-Walking", "Chorus with Christ"

***

## Support and contact details

Please contact Allie Holcombe at alexandra.holcombe@gmail.com or Nicole Sanders at nsanders9022@gmail.com with any questions, concerns, or suggestions.

***

## Technologies Used

This web application uses:
* Nancy
* Mono
* DNVM
* C#
* Razor
* MSSQL & SSMS

***

### License

*This project is licensed under the MIT license.*

Copyright (c) 2017 **_Alexandra Holcombe & Nicole Sanders_**
