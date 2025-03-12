﻿using InverterMon.Server.Persistence.Settings;
using InverterMon.Shared.Models;

namespace InverterMon.Server.Endpoints.Settings.GetSettingValues;

public class Endpoint : EndpointWithoutRequest<CurrentSettings>
{
    public UserSettings UserSettings { get; set; }

    public override void Configure()
    {
        Get("settings/get-setting-values");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        //todo: get values from inverter and send to client

        // var cmd = new GetSettings();
        // cmd.Result.SystemSpec = UserSettings.ToSystemSpec();
        //
        // if (Env.IsDevelopment())
        // {
        //     cmd.Result.ChargePriority = "03";
        //     cmd.Result.MaxACChargeCurrent = "10";
        //     cmd.Result.MaxCombinedChargeCurrent = "020";
        //     cmd.Result.OutputPriority = "02";
        //     cmd.Result.BulkChargeVoltage = 27.1m;
        //     await SendAsync(cmd.Result);
        //     return;
        // }
        //
        // Queue.AddCommands(cmd);
        //
        // await cmd.WhileProcessing(c);
        //
        // if (cmd.IsComplete)
        //     await SendAsync(cmd.Result);
        // else
        //     ThrowError("Unable to read settings in a timely manner!");
    }
}