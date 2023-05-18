Author (Ian Richards)
Version 1.0.0

Api that allows a customer to reserve a parking space over a give date range, cancel a reservation, amend a reservation and check for spaces available over a given date range.

There are unit tests provided for the ParkingManager class as this class is quite complex and during development I needed confidence that any subsequent changes made would not break anything or if it did I would be notified.

The Api has also been manually tested using swagger to ensure the API is running as expected.

The reservation endpoint throw an expcetion if a given date range can not be booked due to insufficent sapces available which is currenty handled in the controller.

If I were to call on with this project I would push the ParkingManager into a ParkingService and have the service return the accept the requests and return the reponses making the controllers as thin as possible.

Total time take on this project over the course of a day was around 5 hours
