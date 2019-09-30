"--header \"Content-Type: application/json\"
--header \"Authorization: bearer \(env.CIRRUS_API_TOKEN)\"
--request POST
--data \"{\\\"query\\\": \\\"query { build(id: \(env.CIRRUS_BUILD_ID)) { tasks { name artifacts { name files { path } } } } }\\\"}\"
--url https://api.cirrus-ci.com/graphql"
