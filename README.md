# Blazor Server Image Recognition App

Simple Blazor Server app which uses image recognition to identify and extract text printed in an image file uploaded by the user.  The app utilises the recognise printed text in image function in the Azure Computer Vision Cognitive service to identify and extract printed text from an uploaded image file

* GUI frontend project developed using Blazor Server/.Net 8
* Image printed text recognition engine is a class library project developed using .Net 8
   * Communicates with the Azure Computer Vision API v3.2
* Unit Test app which unit tests the image printed text recognition engine.  Uses MS Test .Net 8.

## Upload image file to analyse

![](BlazorServerImageRecognitionApp/wwwroot/images/UploadImageFile.png)

## Image file displayed and printed text from image displayed in text area

![](BlazorServerImageRecognitionApp/wwwroot/images/PrintedTextDisplayed.png)
