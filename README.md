# Project AutoLake
Legacy sales systems often break downstream data pipelines whenever schemas change—causing delays, report failures, and manual rework.
Project AutoLake uses Microsoft Fabric Open Mirroring to eliminate data pipeline maintenance entirely.
Schema and data changes from source systems are detected automatically and reflected in the Lakehouse within minutes.
A Fabric Data Agent is then connected to Azure AI Agent Service, enabling natural language queries and real-time analytics without rebuilding models or ETL processes.
This delivers zero downtime, zero pipelines, and instant business insights—all with lower operational overhead.

Repo to show Open mirroring in Fabric and then  chat with your data using Fabric Agent and Azure AI agent Service and show data in Front end in Grid

## Architecture Overview

<img src="images/ArchitectureAutolake.jpeg" width="1000"/>

# Fabric Web App

A web application that demonstrates integration between Microsoft Fabric and Azure AI Agents, allowing users to view mirrored SQL data and interact with it through natural language queries.

## Features

- **Data Visualization**: View mirrored Fabric SQL data from AdventureWorksLT database
- **AI-Powered Chat**: Interact with your data using Azure AI Agents
- **Session Management**: Persistent chat history using ASP.NET Core session state
- **Responsive UI**: Modern interface built with Bootstrap 5

## Prerequisites

- .NET 9.0
- Azure Subscription
- Microsoft Fabric Workspace
- Azure AI Project configured with an agent

## Configuration

The application uses the following configurations:

- **Fabric Database Connection**:
  ```csharp
  Server: "Fabric DataBase Coonection String "
  Database: "AdventureWorksLT_Mirrored"

## Azure AI Agent 
Project Endpoint: "Project endpoint"
Agent ID: "Agent ID visible in playground"

Getting Started
Clone the repository
Ensure you have .NET 9.0 SDK installed
Configure Azure credentials (the app uses DefaultAzureCredential)
Run the application

dotnet run


## Features in Detail

Data Viewing
Navigate to "Show Data" page
Select a table from the dropdown
View mirrored data from Fabric SQL database

## Chat Interface
Go to "Agent Chat" page
Ask natural language questions about your data
View chat history persisted through session

## Authentication
The application uses Azure DefaultAzureCredential for authentication with both Fabric SQL Database and Azure AI Services.

Contributing
Fork the repository
Create a feature branch
Commit your changes
Push to the branch
Create a Pull Request
