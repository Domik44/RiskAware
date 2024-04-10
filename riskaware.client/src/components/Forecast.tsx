import { Component } from 'react';

interface IForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

export class Forecast extends Component<object, { forecasts: IForecast[] | undefined }> {
  constructor(props: object) {
    super(props);
    this.state = {
      forecasts: undefined
    };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  render() {
    const { forecasts } = this.state;
    const contents = forecasts === undefined
      ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
      : <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>;

    return (
      <div>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
    const data: IForecast[] = await response.json();
    this.setState({ forecasts: data });
  }
}
