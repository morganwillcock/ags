def arg(a):                            # Return value for a named argument
$ARGS.named |
getpath([a]);

def arg_flag(a):                       # Check whether a positional argument was set
$ARGS.positional |                     #    e.g. --args name
(index(a) // -1) >= 0;

def arg_true(a):                       # String to boolean conversion for named argument
arg(a) |                               #    e.g. --arg name value
try
  test("(y(es)?|true|1)"; "i")
catch
  false;

def arg_check(a):                      # check argument is 'positive'
arg_flag(a) or arg_true(a);

def curl_option(o):                    # output as curl config
"--\(o) \"\(.)\"";

def uri_encode:                        # URI encode everything except "/"
split("/") |
map(@uri) |
join("/");

.data[].tasks[] |                      # select tasks
select(.artifacts|length>0) |          # select tasks that have artifacts
{ task: .name,                         # create .task (string)
  artifacts: .artifacts[] } |          #        .artifacts (array)
{ task: .task,                         # create .task (string)
  artifact: .artifacts.name,           #        .artifact (string)
  path: .artifacts.files[].path } |    #        .path (string, may contain "/")
join("/") |                            # join as task/artifact/path
select(test(arg("regex") // ".")) |    # regex test the unencoded path
uri_encode |                           # URI encode final string
(arg("basename") // "") + . |          # prepend basename (if set)
if arg_check("curlify") then           # optionally wrap as a curl config option
  curl_option("url")
else
  .
end
