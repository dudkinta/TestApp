﻿namespace CommonLibrary
{
    public class ConsulServiceConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Port { get; set; }
        public string HealthEndpoint { get; set; } = string.Empty;
    }
}
