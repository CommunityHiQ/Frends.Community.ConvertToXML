**[Table of Contents](http://tableofcontent.eu)**
- [Frends.Community.ConvertToXML](#frendscommunityconverttoxml)
  - [Contributing](#contributing)
  - [Documentation](#documentation)
    - [Parameters](#parameters)
    - [CsvInputParameters](#csvinputparameters)
    - [JsonInputParameters](#jsoninputparameters)
    - [Options](#options)
    - [Result](#result)
  - [License](#license)


# Frends.Community.ConvertToXML
FRENDS Task to convert JSON, CSV or fixed length to XML. Some Frends4 tasks already provide method .ToXML() and it is preferable to use it instead of this task.

## Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Documentation

### Parameters

| Property				|  Type   | Description								| Example                     |
|-----------------------|---------|-----------------------------------------|-----------------------------|
| Input					| string	| Supported formats JSON, CSV and fixed length | `first;second;third` |

### CsvInputParameters

| Property				|  Type   | Description								| Example                     |
|-----------------------|---------|-----------------------------------------|-----------------------------|
| CSVSeparator			| CSVSeparator	| CSV separator	| `;` |
| ColumnLengths			| array&lt;int&gt;	| Column lengths of fixed lenght input. These are used if CSV separator is not defined.	| `` |
| InputHasHeaderRow		| bool	| Input has header row	| `false` |
| TrimOuputColumns		| bool	| Trim ouput columns of CVS input	| `true` |

### jsonInputParameters

| Property				|  Type   | Description								| Example                     |
|-----------------------|---------|-----------------------------------------|-----------------------------|
| XMLRootElementName	| string	| Root name for when parsing JSON| `Root`	|
| AppendWith			| string	| Append numeric JSON fields with prefix	| `foo_` |

### Result

| Property      | Type     | Description                      | Example                     |
|---------------|----------|----------------------------------|
| Result        | string   | Result as XML	| `<NewDataSet><Table1><Column1>first</Column1><Column2>second</Column2><Column3>third</Column3></Table1></NewDataSet>` |

## License

This project is licensed under the MIT License - see the LICENSE file for details
