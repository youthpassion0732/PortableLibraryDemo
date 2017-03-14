using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace SalarySearchLibrary
{
    public static class SalarySearch
    {

        #region Public Methods

        //Search Employees By "first name, last name, salary start, salary end, title, cabinet, department"
        public static List<Employee> GetEmployees(string firstName, string LastName, string salaryStart, string salaryEnd, string title, string cabinet, string department)
        {
            try
            {
                List<Employee> employeesList = new List<Employee>();

                //Declaring Variables
                string searchPageUrl = "http://migration.kentucky.gov/opendoorsearch/SalarySearch.aspx";
                string eventTarget = "";
                string eventArgument = "";
                string lastFocus = "";
                string viewStateFieldCount = "2";
                string viewState = "%2FwEPDwUJNzE1NzEzODEyD2QWAmYPZBYCAgMPZBYCAgMPZBYCAgEPZBYEZg9kFgQCDw8QDxYGHg1EYXRhVGV4dEZpZWxkBQtEZXNjcmlwdGlvbh4ORGF0YVZhbHVlRmllbGQFC0Rlc2NyaXB0aW9uHgtfIURhdGFCb3VuZGdkEBUODFtTZWxlY3QgQWxsXRRFY29ub21pYyBEZXZlbG9wbWVudBdFZHVjYXRpb24gYW5kIFdvcmtmb3JjZRZFbmVyZ3kgYW5kIEVudmlyb25tZW50GkZpbmFuY2UgYW5kIEFkbWluaXN0cmF0aW9uEkdlbmVyYWwgR292ZXJubWVudBpIZWFsdGggYW5kIEZhbWlseSBTZXJ2aWNlcxlKdXN0aWNlIGFuZCBQdWJsaWMgU2FmZXR5BUxhYm9yCVBlcnNvbm5lbBFQdWJsaWMgUHJvdGVjdGlvbhNQdWJsaWMgVW5pdmVyc2l0aWVzGlRvdXJpc20sIEFydHMgYW5kIEhlcml0YWdlDlRyYW5zcG9ydGF0aW9uFQ4AFEVjb25vbWljIERldmVsb3BtZW50F0VkdWNhdGlvbiBhbmQgV29ya2ZvcmNlFkVuZXJneSBhbmQgRW52aXJvbm1lbnQaRmluYW5jZSBhbmQgQWRtaW5pc3RyYXRpb24SR2VuZXJhbCBHb3Zlcm5tZW50GkhlYWx0aCBhbmQgRmFtaWx5IFNlcnZpY2VzGUp1c3RpY2UgYW5kIFB1YmxpYyBTYWZldHkFTGFib3IJUGVyc29ubmVsEVB1YmxpYyBQcm90ZWN0aW9uE1B1YmxpYyBVbml2ZXJzaXRpZXMaVG91cmlzbSwgQXJ0cyBhbmQgSGVyaXRhZ2UOVHJhbnNwb3J0YXRpb24UKwMOZ2dnZ2dnZ2dnZ2dnZ2cWAWZkAhEPEA8WBh8ABQtEZXNjcmlwdGlvbh8BBQtEZXNjcmlwdGlvbh8CZ2QQFZ4BDFtTZWxlY3QgQWxsXR5BZ3JpY3VsdHVyYWwgRGV2ZWxvcG1lbnQgQm9hcmQQQXR0b3JuZXkgR2VuZXJhbBpBdWRpdG9yIG9mIFB1YmxpYyBBY2NvdW50cyhCb2FyZCBmb3IgUmVzcGlyYXRvcnkgQ2FyZSBQcmFjdGl0aW9uZXJzFEJvYXJkIG9mIEFjY291bnRhbmN5JkJvYXJkIG9mIEFsY29ob2wgYW5kIERydWcgQWJ1c2UgQ25zbHJzFEJvYXJkIG9mIEF1Y3Rpb25lZXJzEkJvYXJkIG9mIEJhcmJlcmluZyhCb2FyZCBvZiBDZXJ0IGZvciBNYXJyaWFnZSAmIEZhbSBUaHJwc3RzJ0JvYXJkIG9mIENlcnQgb2YgRmVlIEJhc2VkIFBzdHJsIENuc2xycx9Cb2FyZCBvZiBDaGlyb3ByYWN0aWMgRXhhbWluZXJzJEJvYXJkIG9mIENsYWltcyAmIENyaW1lIFZpY3RpbXMgQ29tcBJCb2FyZCBvZiBEZW50aXN0cnkSQm9hcmQgb2YgRWxlY3Rpb25zJUJvYXJkIG9mIEVtYmxtcnMgYW5kIEZ1bmVyYWwgSG9tZSBEaXIjQm9hcmQgb2YgRXhhbWluZXJzIG9mIFBzeWNob2xvZ2lzdHMhQm9hcmQgb2YgRXhhbWluZXJzIG9mIFNvY2lhbCBXb3JrLkJvYXJkIG9mIEV4YW1ycy9SZWcgb2YgTGRzY3AgQXJjaHMgb2YgS2VudHVja3koQm9hcmQgb2YgSGFpcmRyZXNzZXJzIGFuZCBDb3NtZXRvbG9naXN0cxhCb2FyZCBvZiBIb21lIEluc3BlY3RvcnMnQm9hcmQgb2YgSW50cnB0cnMgZm9yIERlYWYgJiBIZCBvZiBIcm5nJEJvYXJkIG9mIExpY2Vuc3VyZSBmb3IgTWFzc2FnZSBUaHJweSZCb2FyZCBvZiBMaWNuZyBIcm5nIEFpZCBEbHJzICYgRml0dGVycylCb2FyZCBvZiBMaWNzdXIgYW5kIENlcnQgZm9yIERpZXQgJiBOdXRycyVCb2FyZCBvZiBMaWNzdXIgZm9yIE5yc2cgSG9tZSBBZG1pbnJzJkJvYXJkIG9mIExpY3N1ciBmb3IgUHJvZiBFbmcgJiBMZCBTdXJ2JEJvYXJkIG9mIExpY3N1ciBmb3IgUHJ2dCBJbnZlc3RpZ29ycxpCb2FyZCBvZiBNZWRpY2FsIExpY2Vuc3VyZRBCb2FyZCBvZiBOdXJzaW5nHUJvYXJkIG9mIE9jY3VwYXRpb25hbCBUaGVyYXB5HUJvYXJkIG9mIE9wdGhhbG1pYyBEaXNwZW5zZXJzHUJvYXJkIG9mIE9wdG9tZXRyaWMgRXhhbWluZXJzEUJvYXJkIG9mIFBoYXJtYWN5G0JvYXJkIG9mIFBoeXNpY2FsIFRoZXJhcGlzdBFCb2FyZCBvZiBQb2RpYXRyeSRCb2FyZCBvZiBQcm9mZXNzaW9uYWwgQXJ0IFRoZXJhcGlzdHMgQm9hcmQgb2YgUHJvZmVzc2lvbmFsIENvdW5zZWxvcnMgQm9hcmQgb2YgUmVnIGZvciBQcm9mIEdlb2xvZ2lzdHMlQm9hcmQgb2YgU3BlZWNoIFBhdGhvbG9neSAmIEF1ZGlvbG9neRRCb2FyZCBvZiBUYXggQXBwZWFscx1Cb2FyZCBvZiBWZXRlcmluYXJ5IEV4YW1pbmVycypDb21taXNzaW9uIGZvciBDaGxkcm4gV3RoIFNwY2wgSGx0aCBDciBOZHMmQ29tbWlzc2lvbiBvbiBEZWFmIGFuZCBIYXJkIG9mIEhlYXJpbmcTQ29tbWlzc2lvbiBvbiBXb21lbiFDb21tb253ZWFsdGggT2ZmaWNlIG9mIFRlY2hub2xvZ3kiQ291bmNpbCBvbiBQb3N0c2Vjb25kYXJ5IEVkdWNhdGlvbitEZXBhcnRtZW50IGZvciBBZ2luZyBhbmQgSW5kZXBlbmRlbnQgTGl2aW5nLERlcGFydG1lbnQgZm9yIEJlaGF2IEhlYWx0aCwgRGV2ICYgSW50IERpc2FiJ0RlcGFydG1lbnQgZm9yIENvbW11bml0eSBCYXNlZCBTZXJ2aWNlcyJEZXBhcnRtZW50IGZvciBFbmVyZ3kgRGV2ICYgSW5kcGRjJ0RlcGFydG1lbnQgZm9yIEVudmlyb25tZW50YWwgUHJvdGVjdGlvbipEZXBhcnRtZW50IGZvciBGYWNpbGl0aWVzIGFuZCBTdXBwb3J0IFNydnMmRGVwYXJ0bWVudCBmb3IgRmFtIFJlcyBDdHJzICYgVm9sIFN2Y3MdRGVwYXJ0bWVudCBmb3IgSW5jb21lIFN1cHBvcnQjRGVwYXJ0bWVudCBmb3IgTGlicmFyaWVzICYgQXJjaGl2ZXMfRGVwYXJ0bWVudCBmb3IgTG9jYWwgR292ZXJubWVudCBEZXBhcnRtZW50IGZvciBNZWRpY2FpZCBTZXJ2aWNlcyBEZXBhcnRtZW50IGZvciBOYXR1cmFsIFJlc291cmNlcx5EZXBhcnRtZW50IGZvciBQdWJsaWMgQWR2b2NhY3kcRGVwYXJ0bWVudCBmb3IgUHVibGljIEhlYWx0aCNEZXBhcnRtZW50IGZvciBXb3JrZm9yY2UgSW52ZXN0bWVudBlEZXBhcnRtZW50IG9mIEFncmljdWx0dXJlKERlcGFydG1lbnQgb2YgQWxjb2hvbGljIEJldmVyYWdlIENvbnRyb2wWRGVwYXJ0bWVudCBvZiBBdmlhdGlvbh9EZXBhcnRtZW50IG9mIENoYXJpdGFibGUgR2FtaW5nGURlcGFydG1lbnQgb2YgQ29ycmVjdGlvbnMnRGVwYXJ0bWVudCBvZiBDcmltaW5hbCBKdXN0aWNlIFRyYWluaW5nF0RlcGFydG1lbnQgb2YgRWR1Y2F0aW9uIERlcGFydG1lbnQgb2YgRW1wbG95ZWUgSW5zdXJhbmNlJERlcGFydG1lbnQgb2YgRmluYW5jaWFsIEluc3RpdHV0aW9ucxZEZXBhcnRtZW50IG9mIEhpZ2h3YXlzKkRlcGFydG1lbnQgb2YgSG91c2luZywgQnVpbGRpbmdzIGFuZCBDb25zdCNEZXBhcnRtZW50IG9mIEh1bWFuIFJlc291cmNlcyBBZG1pbhdEZXBhcnRtZW50IG9mIEluc3VyYW5jZR5EZXBhcnRtZW50IG9mIEp1dmVuaWxlIEp1c3RpY2UeRGVwYXJ0bWVudCBvZiBNaWxpdGFyeSBBZmZhaXJzFURlcGFydG1lbnQgb2YgUmV2ZW51ZSVEZXBhcnRtZW50IG9mIFJ1cmFsIGFuZCBNdW5pY2lwYWwgQWlkIERlcGFydG1lbnQgb2YgVmVoaWNsZSBSZWd1bGF0aW9uHkRlcGFydG1lbnQgb2YgVmV0ZXJhbnMgQWZmYWlycx1EZXBhcnRtZW50IG9mIFdvcmtlcnMnIENsYWltcyFEZXBhcnRtZW50IG9mIFdvcmtwbGFjZSBTdGFuZGFyZHMgRWFybHkgQ2hpbGRob29kIEFkdmlzb3J5IENvdW5jaWwmRWR1Y2F0aW9uIFByb2Zlc3Npb25hbCBTdGFuZGFyZHMgQm9hcmQgRW52aXJvbm1lbnRhbCBRdWFsaXR5IENvbW1pc3Npb24iRXhlY3V0aXZlIEJyYW5jaCBFdGhpY3MgQ29tbWlzc2lvbghHb3Zlcm5vciNHb3ZzIE9mZmljZSBvZiBNaW5vcml0eSBFbXBvd2VybWVudBdIdW1hbiBSaWdodHMgQ29tbWlzc2lvbiFLZW50dWNreSBBcnRpc2FucyBDZW50ZXIgYXQgQmVyZWEVS2VudHVja3kgQXJ0cyBDb3VuY2lsHEtlbnR1Y2t5IEJvYXJkIG9mIEFyY2hpdGVjdHMnS2VudHVja3kgQm94aW5nIGFuZCBXcmVzdGxpbmcgQXV0aG9yaXR5LktlbnR1Y2t5IEJyZCBmb3IgTWVkIEltYWdpbmcgJiBSYWRpYXRpb24gVGhycHksS2VudHVja3kgQ29tbWlzc2lvbiBvbiBQcm9wcmlldGFyeSBFZHVjYXRpb24cS2VudHVja3kgRGVwYXJ0bWVudCBvZiBQYXJrcx1LZW50dWNreSBEZXBhcnRtZW50IG9mIFRyYXZlbB9LZW50dWNreSBFZHVjYXRpb25hbCBUZWxldmlzaW9uKEtlbnR1Y2t5IEVudmlyb25tZW50YWwgRWR1Y2F0aW9uIENvdW5jaWwkS2VudHVja3kgRmlzaCBhbmQgV2lsZGxpZmUgUmVzb3VyY2VzGUtlbnR1Y2t5IEhlcml0YWdlIENvdW5jaWwpS2VudHVja3kgSGlnaGVyIEVkdWMgQXNzaXN0YW5jZSBBdXRob3JpdHksS2VudHVja3kgSGlnaGVyIEVkdWNhdGlvbiBTdHVkZW50IExvYW4gQ29ycC4bS2VudHVja3kgSGlzdG9yaWNhbCBTb2NpZXR5E0tlbnR1Y2t5IEhvcnNlIFBhcmsgS2VudHVja3kgSG9yc2UgUmFjaW5nIENvbW1pc3Npb24VS2VudHVja3kgSG91c2luZyBDb3JwIUtlbnR1Y2t5IEluZnJhc3RydWN0dXJlIEF1dGhvcml0eRZLZW50dWNreSBMb3R0ZXJ5IENvcnAuKUtlbnR1Y2t5IE9jY3VwIFNhZmV0eSAmIEhlYWx0aCBTdGFuZHMgQnJkMUtlbnR1Y2t5IE9mZmljZSBvZiBIbHRoIEJlbmVmaXQgJiBIbHRoIEluZm8gRXhjaGceS2VudHVja3kgT1NIIFJldmlldyBDb21taXNzaW9uIktlbnR1Y2t5IFB1YmxpYyBTZXJ2aWNlIENvbW1pc3Npb24bS2VudHVja3kgUmV0aXJlbWVudCBTeXN0ZW1zGEtlbnR1Y2t5IFJpdmVyIEF1dGhvcml0eRlLZW50dWNreSBTdGF0ZSBGYWlyIEJvYXJkKktlbnR1Y2t5IFN0YXRlIE5hdHVyZSBQcmVzZXJ2ZXMgQ29tbWlzc2lvbhVLZW50dWNreSBTdGF0ZSBQb2xpY2UTTGlldXRlbmFudCBHb3Zlcm5vchtNaWxpdGFyeSBBZmZhaXJzIENvbW1pc3Npb24dTWluZSBTYWZldHkgUmV2aWV3IENvbW1pc3Npb24XTXVycmF5IFN0YXRlIFVuaXZlcnNpdHkOTmF0aW9uYWwgR3VhcmQcTm9ydGhlcm4gS2VudHVja3kgVW5pdmVyc2l0eSVPZmZpY2UgRmFpdGggQnNkICYgQ29tbSBOcHJmIFNvYyBTcnZzEE9mZmljZSBvZiBBdWRpdHMpT2ZmaWNlIG9mIEdlbiBBZG0vUHJvZyBTdXBwIGZvciBTaHJkIFNydnMXT2ZmaWNlIG9mIEhlYWx0aCBQb2xpY3kjT2ZmaWNlIG9mIEh1bWFuIFJlc291cmNlIE1hbmFnZW1lbnQgT2ZmaWNlIG9mIEluZm9ybWF0aW9uIFRlY2hub2xvZ3klT2ZmaWNlIG9mIEluc3BlY3QgR2VuIGZvciBTaGFyZWQgU3J2cxtPZmZpY2Ugb2YgSW5zcGVjdG9yIEdlbmVyYWwYT2ZmaWNlIG9mIExlZ2FsIFNlcnZpY2VzJU9mZmljZSBvZiBPY2N1cGF0aW9ucyBhbmQgUHJvZmVzc2lvbnMPT2ZmaWNlIG9mIFBWQSdzH09mZmljZSBvZiBTdGF0ZSBCdWRnZXQgRGlyZWN0b3IaT2ZmaWNlIG9mIFN1cHBvcnQgU2VydmljZXMYT2ZmaWNlIG9mIHRoZSBDb250cm9sbGVyF09mZmljZSBvZiB0aGUgU2VjcmV0YXJ5IU9mZmljZSBvZiBUcmFuc3BvcnRhdGlvbiBEZWxpdmVyeSZQZXJzIENhYmluZXQgLSBPZmZpY2Ugb2YgdGhlIFNlY3JldGFyeQ9QZXJzb25uZWwgQm9hcmQcUmVhbCBFc3RhdGUgQXBwcmFpc2VycyBCb2FyZBZSZWFsIEVzdGF0ZSBDb21taXNzaW9uHFJlZ2lzdHJ5IG9mIEVsZWN0aW9uIEZpbmFuY2UlU2Nob29sIEZhY2lsaXRpZXMgQ29uc3RydWN0aW9uIENvbWlzcxJTZWNyZXRhcnkgb2YgU3RhdGUYU2VjcmV0YXJ5IG9mIHRoZSBDYWJpbmV0JVN0YXRlIEJvYXJkIGZvciBQcm9wcmlldGFyeSBFZHVjYXRpb24bU3RhdGUgTGFib3IgUmVsYXRpb25zIEJvYXJkD1N0YXRlIFRyZWFzdXJlch9UaGUgT2ZmaWNlIG9mIEhvbWVsYW5kIFNlY3VyaXR5HFVuaWZpZWQgUHJvc2VjdXRvcmlhbCBTeXN0ZW0WVW5pdmVyc2l0eSBvZiBLZW50dWNreRhVbml2ZXJzaXR5IG9mIExvdWlzdmlsbGUbV2VzdGVybiBLZW50dWNreSBVbml2ZXJzaXR5G1dvcmtlcnMgQ29tcCBGdW5kaW5nIENvbWlzcxWeAQAeQWdyaWN1bHR1cmFsIERldmVsb3BtZW50IEJvYXJkEEF0dG9ybmV5IEdlbmVyYWwaQXVkaXRvciBvZiBQdWJsaWMgQWNjb3VudHMoQm9hcmQgZm9yIFJlc3BpcmF0b3J5IENhcmUgUHJhY3RpdGlvbmVycxRCb2FyZCBvZiBBY2NvdW50YW5jeSZCb2FyZCBvZiBBbGNvaG9sIGFuZCBEcnVnIEFidXNlIENuc2xycxRCb2FyZCBvZiBBdWN0aW9uZWVycxJCb2FyZCBvZiBCYXJiZXJpbmcoQm9hcmQgb2YgQ2VydCBmb3IgTWFycmlhZ2UgJiBGYW0gVGhycHN0cydCb2FyZCBvZiBDZXJ0IG9mIEZlZSBCYXNlZCBQc3RybCBDbnNscnMfQm9hcmQgb2YgQ2hpcm9wcmFjdGljIEV4YW1pbmVycyRCb2FyZCBvZiBDbGFpbXMgJiBDcmltZSBWaWN0aW1zIENvbXASQm9hcmQgb2YgRGVudGlzdHJ5EkJvYXJkIG9mIEVsZWN0aW9ucyVCb2FyZCBvZiBFbWJsbXJzIGFuZCBGdW5lcmFsIEhvbWUgRGlyI0JvYXJkIG9mIEV4YW1pbmVycyBvZiBQc3lj";
                string viewState1 = "aG9sb2dpc3RzIUJvYXJkIG9mIEV4YW1pbmVycyBvZiBTb2NpYWwgV29yay5Cb2FyZCBvZiBFeGFtcnMvUmVnIG9mIExkc2NwIEFyY2hzIG9mIEtlbnR1Y2t5KEJvYXJkIG9mIEhhaXJkcmVzc2VycyBhbmQgQ29zbWV0b2xvZ2lzdHMYQm9hcmQgb2YgSG9tZSBJbnNwZWN0b3JzJ0JvYXJkIG9mIEludHJwdHJzIGZvciBEZWFmICYgSGQgb2YgSHJuZyRCb2FyZCBvZiBMaWNlbnN1cmUgZm9yIE1hc3NhZ2UgVGhycHkmQm9hcmQgb2YgTGljbmcgSHJuZyBBaWQgRGxycyAmIEZpdHRlcnMpQm9hcmQgb2YgTGljc3VyIGFuZCBDZXJ0IGZvciBEaWV0ICYgTnV0cnMlQm9hcmQgb2YgTGljc3VyIGZvciBOcnNnIEhvbWUgQWRtaW5ycyZCb2FyZCBvZiBMaWNzdXIgZm9yIFByb2YgRW5nICYgTGQgU3VydiRCb2FyZCBvZiBMaWNzdXIgZm9yIFBydnQgSW52ZXN0aWdvcnMaQm9hcmQgb2YgTWVkaWNhbCBMaWNlbnN1cmUQQm9hcmQgb2YgTnVyc2luZx1Cb2FyZCBvZiBPY2N1cGF0aW9uYWwgVGhlcmFweR1Cb2FyZCBvZiBPcHRoYWxtaWMgRGlzcGVuc2Vycx1Cb2FyZCBvZiBPcHRvbWV0cmljIEV4YW1pbmVycxFCb2FyZCBvZiBQaGFybWFjeRtCb2FyZCBvZiBQaHlzaWNhbCBUaGVyYXBpc3QRQm9hcmQgb2YgUG9kaWF0cnkkQm9hcmQgb2YgUHJvZmVzc2lvbmFsIEFydCBUaGVyYXBpc3RzIEJvYXJkIG9mIFByb2Zlc3Npb25hbCBDb3Vuc2Vsb3JzIEJvYXJkIG9mIFJlZyBmb3IgUHJvZiBHZW9sb2dpc3RzJUJvYXJkIG9mIFNwZWVjaCBQYXRob2xvZ3kgJiBBdWRpb2xvZ3kUQm9hcmQgb2YgVGF4IEFwcGVhbHMdQm9hcmQgb2YgVmV0ZXJpbmFyeSBFeGFtaW5lcnMqQ29tbWlzc2lvbiBmb3IgQ2hsZHJuIFd0aCBTcGNsIEhsdGggQ3IgTmRzJkNvbW1pc3Npb24gb24gRGVhZiBhbmQgSGFyZCBvZiBIZWFyaW5nE0NvbW1pc3Npb24gb24gV29tZW4hQ29tbW9ud2VhbHRoIE9mZmljZSBvZiBUZWNobm9sb2d5IkNvdW5jaWwgb24gUG9zdHNlY29uZGFyeSBFZHVjYXRpb24rRGVwYXJ0bWVudCBmb3IgQWdpbmcgYW5kIEluZGVwZW5kZW50IExpdmluZyxEZXBhcnRtZW50IGZvciBCZWhhdiBIZWFsdGgsIERldiAmIEludCBEaXNhYidEZXBhcnRtZW50IGZvciBDb21tdW5pdHkgQmFzZWQgU2VydmljZXMiRGVwYXJ0bWVudCBmb3IgRW5lcmd5IERldiAmIEluZHBkYydEZXBhcnRtZW50IGZvciBFbnZpcm9ubWVudGFsIFByb3RlY3Rpb24qRGVwYXJ0bWVudCBmb3IgRmFjaWxpdGllcyBhbmQgU3VwcG9ydCBTcnZzJkRlcGFydG1lbnQgZm9yIEZhbSBSZXMgQ3RycyAmIFZvbCBTdmNzHURlcGFydG1lbnQgZm9yIEluY29tZSBTdXBwb3J0I0RlcGFydG1lbnQgZm9yIExpYnJhcmllcyAmIEFyY2hpdmVzH0RlcGFydG1lbnQgZm9yIExvY2FsIEdvdmVybm1lbnQgRGVwYXJ0bWVudCBmb3IgTWVkaWNhaWQgU2VydmljZXMgRGVwYXJ0bWVudCBmb3IgTmF0dXJhbCBSZXNvdXJjZXMeRGVwYXJ0bWVudCBmb3IgUHVibGljIEFkdm9jYWN5HERlcGFydG1lbnQgZm9yIFB1YmxpYyBIZWFsdGgjRGVwYXJ0bWVudCBmb3IgV29ya2ZvcmNlIEludmVzdG1lbnQZRGVwYXJ0bWVudCBvZiBBZ3JpY3VsdHVyZShEZXBhcnRtZW50IG9mIEFsY29ob2xpYyBCZXZlcmFnZSBDb250cm9sFkRlcGFydG1lbnQgb2YgQXZpYXRpb24fRGVwYXJ0bWVudCBvZiBDaGFyaXRhYmxlIEdhbWluZxlEZXBhcnRtZW50IG9mIENvcnJlY3Rpb25zJ0RlcGFydG1lbnQgb2YgQ3JpbWluYWwgSnVzdGljZSBUcmFpbmluZxdEZXBhcnRtZW50IG9mIEVkdWNhdGlvbiBEZXBhcnRtZW50IG9mIEVtcGxveWVlIEluc3VyYW5jZSREZXBhcnRtZW50IG9mIEZpbmFuY2lhbCBJbnN0aXR1dGlvbnMWRGVwYXJ0bWVudCBvZiBIaWdod2F5cypEZXBhcnRtZW50IG9mIEhvdXNpbmcsIEJ1aWxkaW5ncyBhbmQgQ29uc3QjRGVwYXJ0bWVudCBvZiBIdW1hbiBSZXNvdXJjZXMgQWRtaW4XRGVwYXJ0bWVudCBvZiBJbnN1cmFuY2UeRGVwYXJ0bWVudCBvZiBKdXZlbmlsZSBKdXN0aWNlHkRlcGFydG1lbnQgb2YgTWlsaXRhcnkgQWZmYWlycxVEZXBhcnRtZW50IG9mIFJldmVudWUlRGVwYXJ0bWVudCBvZiBSdXJhbCBhbmQgTXVuaWNpcGFsIEFpZCBEZXBhcnRtZW50IG9mIFZlaGljbGUgUmVndWxhdGlvbh5EZXBhcnRtZW50IG9mIFZldGVyYW5zIEFmZmFpcnMdRGVwYXJ0bWVudCBvZiBXb3JrZXJzJyBDbGFpbXMhRGVwYXJ0bWVudCBvZiBXb3JrcGxhY2UgU3RhbmRhcmRzIEVhcmx5IENoaWxkaG9vZCBBZHZpc29yeSBDb3VuY2lsJkVkdWNhdGlvbiBQcm9mZXNzaW9uYWwgU3RhbmRhcmRzIEJvYXJkIEVudmlyb25tZW50YWwgUXVhbGl0eSBDb21taXNzaW9uIkV4ZWN1dGl2ZSBCcmFuY2ggRXRoaWNzIENvbW1pc3Npb24IR292ZXJub3IjR292cyBPZmZpY2Ugb2YgTWlub3JpdHkgRW1wb3dlcm1lbnQXSHVtYW4gUmlnaHRzIENvbW1pc3Npb24hS2VudHVja3kgQXJ0aXNhbnMgQ2VudGVyIGF0IEJlcmVhFUtlbnR1Y2t5IEFydHMgQ291bmNpbBxLZW50dWNreSBCb2FyZCBvZiBBcmNoaXRlY3RzJ0tlbnR1Y2t5IEJveGluZyBhbmQgV3Jlc3RsaW5nIEF1dGhvcml0eS5LZW50dWNreSBCcmQgZm9yIE1lZCBJbWFnaW5nICYgUmFkaWF0aW9uIFRocnB5LEtlbnR1Y2t5IENvbW1pc3Npb24gb24gUHJvcHJpZXRhcnkgRWR1Y2F0aW9uHEtlbnR1Y2t5IERlcGFydG1lbnQgb2YgUGFya3MdS2VudHVja3kgRGVwYXJ0bWVudCBvZiBUcmF2ZWwfS2VudHVja3kgRWR1Y2F0aW9uYWwgVGVsZXZpc2lvbihLZW50dWNreSBFbnZpcm9ubWVudGFsIEVkdWNhdGlvbiBDb3VuY2lsJEtlbnR1Y2t5IEZpc2ggYW5kIFdpbGRsaWZlIFJlc291cmNlcxlLZW50dWNreSBIZXJpdGFnZSBDb3VuY2lsKUtlbnR1Y2t5IEhpZ2hlciBFZHVjIEFzc2lzdGFuY2UgQXV0aG9yaXR5LEtlbnR1Y2t5IEhpZ2hlciBFZHVjYXRpb24gU3R1ZGVudCBMb2FuIENvcnAuG0tlbnR1Y2t5IEhpc3RvcmljYWwgU29jaWV0eRNLZW50dWNreSBIb3JzZSBQYXJrIEtlbnR1Y2t5IEhvcnNlIFJhY2luZyBDb21taXNzaW9uFUtlbnR1Y2t5IEhvdXNpbmcgQ29ycCFLZW50dWNreSBJbmZyYXN0cnVjdHVyZSBBdXRob3JpdHkWS2VudHVja3kgTG90dGVyeSBDb3JwLilLZW50dWNreSBPY2N1cCBTYWZldHkgJiBIZWFsdGggU3RhbmRzIEJyZDFLZW50dWNreSBPZmZpY2Ugb2YgSGx0aCBCZW5lZml0ICYgSGx0aCBJbmZvIEV4Y2hnHktlbnR1Y2t5IE9TSCBSZXZpZXcgQ29tbWlzc2lvbiJLZW50dWNreSBQdWJsaWMgU2VydmljZSBDb21taXNzaW9uG0tlbnR1Y2t5IFJldGlyZW1lbnQgU3lzdGVtcxhLZW50dWNreSBSaXZlciBBdXRob3JpdHkZS2VudHVja3kgU3RhdGUgRmFpciBCb2FyZCpLZW50dWNreSBTdGF0ZSBOYXR1cmUgUHJlc2VydmVzIENvbW1pc3Npb24VS2VudHVja3kgU3RhdGUgUG9saWNlE0xpZXV0ZW5hbnQgR292ZXJub3IbTWlsaXRhcnkgQWZmYWlycyBDb21taXNzaW9uHU1pbmUgU2FmZXR5IFJldmlldyBDb21taXNzaW9uF011cnJheSBTdGF0ZSBVbml2ZXJzaXR5Dk5hdGlvbmFsIEd1YXJkHE5vcnRoZXJuIEtlbnR1Y2t5IFVuaXZlcnNpdHklT2ZmaWNlIEZhaXRoIEJzZCAmIENvbW0gTnByZiBTb2MgU3J2cxBPZmZpY2Ugb2YgQXVkaXRzKU9mZmljZSBvZiBHZW4gQWRtL1Byb2cgU3VwcCBmb3IgU2hyZCBTcnZzF09mZmljZSBvZiBIZWFsdGggUG9saWN5I09mZmljZSBvZiBIdW1hbiBSZXNvdXJjZSBNYW5hZ2VtZW50IE9mZmljZSBvZiBJbmZvcm1hdGlvbiBUZWNobm9sb2d5JU9mZmljZSBvZiBJbnNwZWN0IEdlbiBmb3IgU2hhcmVkIFNydnMbT2ZmaWNlIG9mIEluc3BlY3RvciBHZW5lcmFsGE9mZmljZSBvZiBMZWdhbCBTZXJ2aWNlcyVPZmZpY2Ugb2YgT2NjdXBhdGlvbnMgYW5kIFByb2Zlc3Npb25zD09mZmljZSBvZiBQVkEncx9PZmZpY2Ugb2YgU3RhdGUgQnVkZ2V0IERpcmVjdG9yGk9mZmljZSBvZiBTdXBwb3J0IFNlcnZpY2VzGE9mZmljZSBvZiB0aGUgQ29udHJvbGxlchdPZmZpY2Ugb2YgdGhlIFNlY3JldGFyeSFPZmZpY2Ugb2YgVHJhbnNwb3J0YXRpb24gRGVsaXZlcnkmUGVycyBDYWJpbmV0IC0gT2ZmaWNlIG9mIHRoZSBTZWNyZXRhcnkPUGVyc29ubmVsIEJvYXJkHFJlYWwgRXN0YXRlIEFwcHJhaXNlcnMgQm9hcmQWUmVhbCBFc3RhdGUgQ29tbWlzc2lvbhxSZWdpc3RyeSBvZiBFbGVjdGlvbiBGaW5hbmNlJVNjaG9vbCBGYWNpbGl0aWVzIENvbnN0cnVjdGlvbiBDb21pc3MSU2VjcmV0YXJ5IG9mIFN0YXRlGFNlY3JldGFyeSBvZiB0aGUgQ2FiaW5ldCVTdGF0ZSBCb2FyZCBmb3IgUHJvcHJpZXRhcnkgRWR1Y2F0aW9uG1N0YXRlIExhYm9yIFJlbGF0aW9ucyBCb2FyZA9TdGF0ZSBUcmVhc3VyZXIfVGhlIE9mZmljZSBvZiBIb21lbGFuZCBTZWN1cml0eRxVbmlmaWVkIFByb3NlY3V0b3JpYWwgU3lzdGVtFlVuaXZlcnNpdHkgb2YgS2VudHVja3kYVW5pdmVyc2l0eSBvZiBMb3Vpc3ZpbGxlG1dlc3Rlcm4gS2VudHVja3kgVW5pdmVyc2l0eRtXb3JrZXJzIENvbXAgRnVuZGluZyBDb21pc3MUKwOeAWdnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZGQCAQ9kFgICAQ88KwANAGQYAgUxY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRSZXBvcnRDcml0ZXJpYU11bHRpVmlldw8PZGZkBS9jdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJFJlcG9ydFJlc3VsdHNHcmlkVmlldw9nZC0ySHVfveSKzPF4AV3Xlk7LHnrS";
                string eventValidation = "%2FwEWtQECvauzqQECofeXogMCmZjpwQMC1NDXywkCnYPr8wUCxMrjlgICj%2FW%2B4AUCj%2FW%2B4AUC6Yr7ggUC2LHkrQkCrej4ywoC79n91AQC2tjX4gECweqtkw0CsL6EzQkC47TikQcC85e1hQwC96qrjQ4C5tGY2QICpLOrkQsCyvvX2QoC7P%2FqiwkCr%2BuavQ8ChMLlgwkCpcTQgg0C1YWdgwcC%2BsHgHwLS0vGeCQKPwNaDDALIl7z5DAKugc6DAQL78%2BipDgLSqs%2F1DALF66aACQL59Jr1BQLr%2BafRDQL%2B6quJDwKTrOXEDQLN28OlBwKK8N7xDAKp%2FZNMArC6904Cs86ctgYCvYrhjQ0CnY2%2BlwwCkeXK8w0Cks2j5QkCnp%2BV6woCwJXx3QoCjPPiywkC8beI3g0CoLfe8AoC%2BszF9wMCwoTonAoClYiQ1Q8CxKuKhQQCvJq8wQgCrcnLtQICoN%2BXggMCipTv0wIC4emiuggC04WOgAECwIu8ogYClN7qwA0CtIe2rwgCgIuu5Q8CndzFpAwC9NKnzAoC4eG3pQ8C%2B8zhvAcCp6iBxwcCxLL7jggCkvPq2w0CoMTQ1g8CuI6chAkClqua8Q0C%2FN7PgQMC1dzdzwcC1t6QnAECtbq0twYCt4eEiAIChOursgoCxLCGqw8Cv7KEgwkCr4TP%2FQgCmqT9wAcCmY3roAgCv%2BG7vg4Cz87SlAoCkobo%2BQ0C7M7oiwYCh96o0AgCtqWwlQsCvaabygMCp%2BrXhQ4C6bjVlQcCmJ3qxAoC7Kzt7wgCwu%2FLhAUCyKaLjQ8CgPbR2QMCysShnQgC0r2CqQoCua3ojgsCvZfJ7g8CrJHb3AsC%2B8fzyA8Cwr2qvgwCoOvnkAgCwo%2BqnwcCg9%2B2iwcClNuP3wwCpdD08Q8CutOWhQcC4p2RnQsC0%2FKGiAMC94CpiQQC0%2FjUyg4C3v%2FcswUChN7WrAEC7IGO9gkCkvHKswoCkZHopwkCiceijgMClumdmgUCiPzR0A8C1vGlpgYCooPqdQKGhofECQK%2Fs6vSCQLj%2BMpmApm367sIApqt5PQOAu%2B74%2BMNAtLBiJ0HAs3R28EIAvTXvK4OAqnw9JYOAqXcmtgMAqiKjKYNAt%2Fu1rMOApjVkBcC8L%2BzwQYC65y2hgsC2PSc1gsClLWc7g8Ch5OItAMCnrzZ8w0C4pPplgIChpDP4wsC%2FY%2FLpQgC7JTzmg8C1PuX0QkCxZTP6wQCg9epiAYC89zJlwIC55aClgwCw73tiQ8C%2Fc7BVALQzvL3BAKtv8O%2BBwKFgt6DDgLx1ZCECwL%2BheOkDgLkocnMAQKX6Pn5BALksavTDALYtuHoCQKg%2BcnYBALnq%2Fj6DwLfmebxBwKClu2FDQLW5Jb1BALQoPOrBgLAgYWVCQLNqI%2F1CALEpq2lCgK%2BoeHwDgL9gfTaAQLd7rfLBwKbsZ3PCmXvYJGkx9sMBZWMxQ5LTihfdhDf";

                //Setting Data For Posting 
                string postData = "__EVENTTARGET={0}&__EVENTARGUMENT={1}&__LASTFOCUS={2}&__VIEWSTATEFIELDCOUNT={3}&__VIEWSTATE={4}&__VIEWSTATE1={5}&__EVENTVALIDATION={6}&ctl00%24ContentPlaceHolder1%24textboxFirstName={7}&ctl00%24ContentPlaceHolder1%24textboxLastName={8}&ctl00%24ContentPlaceHolder1%24textboxSalaryStartRange={9}&ctl00%24ContentPlaceHolder1%24textboxSalaryEndRange={10}&ctl00%24ContentPlaceHolder1%24textboxTitle={11}&ctl00%24ContentPlaceHolder1%24CabinetDropDownList={12}&ctl00%24ContentPlaceHolder1%24DepartmentDropDownList={13}&ctl00%24ContentPlaceHolder1%24GenerateReportButton=Search";
                postData = postData.Replace("{0}", eventTarget);
                postData = postData.Replace("{1}", eventArgument);
                postData = postData.Replace("{2}", lastFocus);
                postData = postData.Replace("{3}", viewStateFieldCount);
                postData = postData.Replace("{4}", viewState);
                postData = postData.Replace("{5}", viewState1);
                postData = postData.Replace("{6}", eventValidation);
                postData = postData.Replace("{7}", firstName.Trim());
                postData = postData.Replace("{8}", LastName.Trim());
                postData = postData.Replace("{9}", salaryStart.Trim());
                postData = postData.Replace("{10}", salaryEnd.Trim());
                postData = postData.Replace("{11}", title.Trim());
                postData = postData.Replace("{12}", cabinet.Trim());
                postData = postData.Replace("{13}", department.Trim());

                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, searchPageUrl);
                    StringContent content = new StringContent(postData, null, "application/x-www-form-urlencoded");

                    //Setting Headers
                    request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.Headers.Add("Cache-Control", "max-age=0");
                    request.Headers.Add("Connection", "keep-alive");
                    request.Headers.Add("Host", "migration.kentucky.gov");
                    request.Headers.Add("Origin", "http://migration.kentucky.gov");
                    request.Headers.Add("Referer", searchPageUrl);
                    request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36");
                    content.Headers.Add("Content-Length", Encoding.UTF8.GetBytes(postData).Length.ToString());

                    //Assigning Content to Request
                    request.Content = content;

                    //Sending Request To Post Search Form
                    var response = httpClient.SendAsync(request).Result;

                    //Reading Search Response
                    var result = response.Content.ReadAsStringAsync().Result;

                    //Removing Double Quotes
                    result = result.Replace("\"", "");

                    //Setting Same Class for all GridView Rows
                    result = result.Replace("gridViewAltRow", "gridViewRow");

                    //Setting List of Employees
                    employeesList = ParseResponseString(result);
                }

                return employeesList;
            }
            catch (Exception error)
            {
                throw new Exception("Employee Search Process Failed. Error: " + error.Message);
            }
        }

        //Getting DropDown Values of "Cabinet"
        public static List<string> GetCabinets()
        {
            try
            {
                //Getting "Cabinet" Dropdown values
                List<string> cabinetsList = GetDropDownValues("Cabinet");

                return cabinetsList;
            }
            catch (Exception error)
            {
                throw new Exception("Getting Cabinets List Process Failed. Error: " + error.Message); 
            }
        }

        //Getting DropDown Values of "Department"
        public static List<string> GetDepartments()
        {
            try
            {
                //Getting "Department" Dropdown values
                List<string> departmentList = GetDropDownValues("Department");

                return departmentList;
            }
            catch (Exception error)
            {
                throw new Exception("Getting Departments List Process Failed. Error: " + error.Message);
            }
        }

        #endregion

        #region Private Methods

        //Parsing Search Page Response To Set List of Employees
        private static List<Employee> ParseResponseString(string responseMessage)
        {
            try
            {
                List<Employee> employeesList = new List<Employee>();

                //Getting Grid Rows
                MatchCollection mcGridRow = Regex.Matches(responseMessage, "<tr class=gridViewRow>(.*?)</tr>", RegexOptions.Singleline);

                foreach (Match rowMatch in mcGridRow)
                {
                    string gridRowString = rowMatch.Result("$1").ToString();

                    //Getting Grid Columns of Row
                    MatchCollection mcGridCol = Regex.Matches(gridRowString, "<td(.*?)>(.*?)</td><td(.*?)>(.*?)</td><td(.*?)>(.*?)</td><td(.*?)>(.*?)</td><td(.*?)>(.*?)</td>", RegexOptions.None);
                    if (mcGridCol.Count > 0)
                    {
                        string fullName = mcGridCol[mcGridCol.Count - 1].Result("$2").ToString();
                        string[] nameWords = fullName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                        if (nameWords.Length > 0)
                        {
                            //Setting Employee Values
                            Employee emp = new Employee();
                            emp.firstName = nameWords[0].ToString();
                            emp.lastName = nameWords[nameWords.Length - 1].ToString();
                            emp.title = mcGridCol[mcGridCol.Count - 1].Result("$4").ToString();
                            emp.cabinet = mcGridCol[mcGridCol.Count - 1].Result("$6").ToString();
                            emp.department = mcGridCol[mcGridCol.Count - 1].Result("$8").ToString();
                            emp.salary = mcGridCol[mcGridCol.Count - 1].Result("$10").ToString();

                            //Adding New Employee To List
                            employeesList.Add(emp);
                        }
                    }
                }                

                return employeesList;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Downloading Html Page String Against Specific Url
        private static string GetPageString(string url)
        {
            try
            {
                var client = new HttpClient();
                var response = client.GetAsync(url).Result;
                var data = response.Content.ReadAsStringAsync().Result;

                return data.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Getting Values of Specific DropDownList
        private static List<string> GetDropDownValues(string dropDownName)
        {
            try
            {
                string dropDownHTML = "";

                List<string> valuesList = new List<string>();

                //Downloading Page String
                string pageString = GetPageString("http://migration.kentucky.gov/opendoorsearch/SalarySearch.aspx");

                //Removing Double Quotes
                pageString = pageString.Replace("\"", "");

                //Getting Dropdown HTML
                MatchCollection mcHtml = Regex.Matches(pageString, "<div class=formLabelWide>(.*?)</div>", RegexOptions.Singleline);
                if (mcHtml.Count > 0)
                {
                    if (dropDownName.ToLower().Equals("cabinet"))
                        dropDownHTML = mcHtml[0].Result("$1").ToString();
                    else if (dropDownName.ToLower().Equals("department"))
                        dropDownHTML = mcHtml[mcHtml.Count - 1].Result("$1").ToString();

                    //Getting Dropdown Values
                    MatchCollection mcValues = Regex.Matches(dropDownHTML, "<option (.*?)>(.*?)</option>", RegexOptions.Singleline);

                    foreach (Match matchValue in mcValues)
                    {
                        //Populating List
                        valuesList.Add(matchValue.Result("$2").ToString());
                    }
                }

                return valuesList;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        #endregion

    }

}
