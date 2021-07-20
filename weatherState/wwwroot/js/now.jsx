function createRemarkable() {
    var remarkable =
        'undefined' != typeof global && global.Remarkable
            ? global.Remarkable
            : window.Remarkable;
    return new remarkable();
}
class CommentBox extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: [] };
        this.handleCommentSubmit = this.handleCommentSubmit.bind(this);
    }
    loadCommentsFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = () => {
            const data = [JSON.parse(xhr.responseText)];
            this.setState({ data: data });
        };
        xhr.send();
    }
    handleCommentSubmit(comment) {
        const data = new FormData();
        data.append('City', comment.city);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = () => this.loadCommentsFromServer();
        xhr.send(data);
    }
    componentDidMount() {
        this.loadCommentsFromServer();
        window.setInterval(
            () => this.loadCommentsFromServer(),
            this.props.pollInterval,
        );
    }
    render() {
        return (
            <div className="commentBox">
                <h1>Comments</h1>
                <CommentList data={this.state.data} />
                <CommentForm onCommentSubmit={this.handleCommentSubmit} />
            </div>
        );
    }
}
class CommentList extends React.Component {
    render() {
        console.log(this.props.data);
        const commentNodes = this.props.data.map(comment => (
            <Comment author={comment.name} tem={comment.temp}>
                {comment.country}
                {comment.status}
                {comment.description}
                {comment.feels_like}
                {comment.pressure}
                {comment.humidity}
            </Comment>
        ));
        return <div className="CommentList">{commentNodes}</div>;
    }
}
class CommentForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { city: '' };
        this.handleCityChange = this.handleCityChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleCityChange(e) {
        this.setState({ city: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        const city = this.state.city.trim();
        if (!city) {
            return;
        }
        this.props.onCommentSubmit({ city: city });
        this.setState({ city: '' });
    }
    render() {
        return (
            <form className="commentForm" onSubmit={this.handleSubmit}>
                <input type="text" placeholder="City" value={this.state.city} onChange={this.handleCityChange} />
                <input type="submit" value="Post" />
            </form>
        );
    }
}
class Comment extends React.Component {
    rawMarkup() {
        const md = createRemarkable();
        const rawMarkup = md.render(this.props.children.toString());
        return { __html: rawMarkup };
    }
    render() {        
        return (
            <div className="comment">
                <h2 className="commentAuthor">{this.props.author}</h2>
                <h3 className="commentAuthor">{this.props.tem}</h3>
                <span dangerouslySetInnerHTML={this.rawMarkup()} />
            </div>
        );
    }
}
/*const data = [
    { id: 1, author: 'Daniel Lo Nigro', text: 'Hello ReactJS.NET World!' },
    { id: 2, author: 'Pete Hunt', text: 'This is one comment' },
    { id: 3, author: 'Jordan Walke', text: 'This is *another* comment' },
];

var xhReq = new XMLHttpRequest();
xhReq.open("GET", "/queries", false);
xhReq.send(null);
var jsonObject = [JSON.parse(xhReq.responseText)];*/


ReactDOM.render(<CommentBox url="/queries" submitUrl="/queries/new" pollInterval={2000} />, document.getElementById('content'));