namespace constants
{
    public static class constants
    {
        public const string accDB =
        @"{
            ""accessories"" : [
                {
                    ""aid"" : 1,
                    ""services"" : [
                        {
                            ""type"" : ""3E"",
                            ""iid"" : 1,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""23"",
                                    ""value"" : ""TestHAP"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 2
                                },
                                {
                                    ""type"" : ""20"",
                                    ""value"" : ""Eric Co"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 3
                                },
                                {
                                    ""type"" : ""30"",
                                    ""value"" : ""037A2BABF19D"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 4
                                },
                                {
                                    ""type"" : ""21"",
                                    ""value"" : ""Device1,1"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 5
                                },
                                {
                                    ""type"" : ""14"",
                                    ""perms"" : [ ""pw"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 6
                                },
                                {
                                    ""type"" : ""52"",
                                    ""value"" : ""100.1.1"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 7
                                },
                                {
                                    ""type"" : ""53"",
                                    ""value"" : ""100.1.1"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 8
                                },
                                {
                                    ""type"" : ""34AB8811-AC7F-4340-BAC3-FD6A85F9943B"",
                                    ""value"" : ""Internal"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 9
                                }
                                    
                            ]
                        },
                        {
                            ""type"" : ""A2"",
                            ""iid"" : 10,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""37"",
                                    ""value"" : ""1.1.0"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 11
                                }
                            ]
                        },
                        {
                            ""type"" : ""47"",
                            ""iid"" : 12,
                            ""primary"" : true, 
                            ""characteristics"" : [
                                {
                                    ""type"" : ""25"",
                                    ""value"" : false,
                                    ""perms"" : [ ""pr"", ""pw"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 13
                                },
                                {
                                    ""type"" : ""23"",
                                    ""value"" : ""Button"",
                                    ""perms"" : [ ""pr""],
                                    ""format"" : ""string"",
                                    ""iid"" : 14
                                },
                                {
                                    ""type"" : ""26"",
                                    ""value"" : false,
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 15
                                }
                                
                            ]
                        }
                    ]
                }
            ]
        }";
        public const string accDBtest =
        @"{
            ""accessories"" : [
                {
                    ""aid"" : 1,
                    ""services"" : [
                        {
                            ""type"" : ""3E"",
                            ""iid"" : 1,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""23"",
                                    ""value"" : ""TestHAP"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 2
                                },
                                {
                                    ""type"" : ""20"",
                                    ""value"" : ""Acme"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 3
                                },
                                {
                                    ""type"" : ""30"",
                                    ""value"" : ""037A2BABF19D"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 4
                                },
                                {
                                    ""type"" : ""21"",
                                    ""value"" : ""Bridge1,1"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 5
                                },
                                {
                                    ""type"" : ""14"",
                                    ""value"" : null,
                                    ""perms"" : [ ""pw"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 6
                                },
                                {
                                    ""type"" : ""52"",
                                    ""value"" : ""100.1.1"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 7
                                }
                            ]
                        },
                        {
                            ""type"" : ""A2"",
                            ""iid"" : 8,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""37"",
                                    ""value"" : ""1.1.0"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 9
                                }
                            ]
                        }
                    ]
                },
                {
                    ""aid"" : 2,
                    ""services"" : [
                        {
                            ""type"" : ""3E"",
                            ""iid"" : 1,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""23"",
                                    ""value"" : ""Acme LED Light Bulb"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 2
                                },
                                {
                                    ""type"" : ""20"",
                                    ""value"" : ""Acme"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 3
                                },
                                {
                                    ""type"" : ""30"",
                                    ""value"" : ""099DB48E9E28"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 4
                                },
                                {
                                    ""type"" : ""21"",
                                    ""value"" : ""LEDBulb1,1"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 5
                                },
                                {
                                    ""type"" : ""14"",
                                    ""value"" : null,
                                    ""perms"" : [ ""pw"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 6
                                }
                            ]
                        },
                        {
                            ""type"" : ""43"",
                            ""iid"" : 7,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""25"",
                                    ""value"" : true,
                                    ""perms"" : [ ""pr"", ""pw"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 8
                                },
                                {
                                    ""type"" : ""8"",
                                    ""value"" : 50,
                                    ""perms"" : [ ""pr"", ""pw"" ],
                                    ""iid"" : 9,
                                    ""maxValue"" : 100,
                                    ""minStep"" : 1,
                                    ""minValue"" : 20,
                                    ""format"" : ""int"",
                                    ""unit"" : ""percentage""
                                }
                            ]
                        }
                    ]
                },
                {
                    ""aid"" : 3,
                    ""services"" : [
                        {
                            ""type"" : ""3E"",
                            ""iid"" : 1,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""23"",
                                    ""value"" : ""Acme LED Light Bulb"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 2
                                },
                                {
                                    ""type"" : ""20"",
                                    ""value"" : ""Acme"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 3
                                },
                                {
                                    ""type"" : ""30"",
                                    ""value"" : ""099DB48E9E28"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 4
                                },
                                {
                                    ""type"" : ""21"",
                                    ""value"" : ""LEDBulb1,1"",
                                    ""perms"" : [ ""pr"" ],
                                    ""format"" : ""string"",
                                    ""iid"" : 5
                                },
                                {
                                    ""type"" : ""14"",
                                    ""value"" : null,
                                    ""perms"" : [ ""pw"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 6
                                }
                            ]
                        },
                        {
                            ""type"" : ""43"",
                            ""iid"" : 7,
                            ""characteristics"" : [
                                {
                                    ""type"" : ""25"",
                                    ""value"" : true,
                                    ""perms"" : [ ""pr"", ""pw"" ],
                                    ""format"" : ""bool"",
                                    ""iid"" : 8
                                },
                                {
                                    ""type"" : ""8"",
                                    ""value"" : 50,
                                    ""perms"" : [ ""pr"", ""pw"" ],
                                    ""iid"" : 9,
                                    ""maxValue"" : 100,
                                    ""minStep"" : 1,
                                    ""minValue"" : 20,
                                    ""format"" : ""int"",
                                    ""unit"" : ""percentage""
                                }
                            ]
                        }
                    ]
                }
            ]
        }";
    }

}


